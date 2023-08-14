using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
namespace Southport.Messaging.Phone.Vonage.Shared.Jwt;

public record VonageToken(double ExpiresAt, string Token)
{
    public double ExpiresAt { get; } = ExpiresAt;
    public string Token { get; } = Token;
}

public static class JwtGenerator
{
    private static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly Dictionary<string, VonageToken> Tokens = new();

    public static string Generate(string privateKey, string applicationId, int expiresInSeconds = 1800,
        List<AclPath> accessControls = null)
    {
        var currentTime = DateTime.UtcNow - EpochTime;
        var currentTimeSeconds = (long)currentTime.TotalSeconds;

        if (Tokens.ContainsKey(applicationId))
        {
            if (Tokens[applicationId].ExpiresAt > currentTimeSeconds + 60)
            {
                return Tokens[applicationId].Token;
            }
        }

        var acls = new Acls
            { Paths = accessControls ?? new List<AclPath> { Acls.Sessions, Acls.Conversations, Acls.Image } };
        var exp = currentTime.Add(TimeSpan.FromSeconds(expiresInSeconds));
        var expTotalSeconds = (long)exp.TotalSeconds;

        var claims = new List<Claim>
        {
            new Claim("application_id", applicationId),
            new Claim("iat", currentTimeSeconds.ToString()),
            new Claim("exp", expTotalSeconds.ToString()),
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("acls", JsonConvert.SerializeObject(acls))
        };

        var rsa = SetupRsaParameters(privateKey);

        var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        var jwt = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credentials
        );

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.WriteToken(jwt);

        Tokens[applicationId] = new VonageToken(expTotalSeconds, jwtToken);
        return jwtToken;

    }

    public static string Decode(string jwt, string privateKey)
    {
        using var rsa = SetupRsaParameters(privateKey);

        var jwtHandler = new JwtSecurityTokenHandler();
        var rsaSecurityKey = new RsaSecurityKey(rsa);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = rsaSecurityKey,
            ValidateIssuer = false,
            ValidateAudience = false
        };


        jwtHandler.ValidateToken(jwt, validationParameters, out var validatedToken);

        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        {
            // You can access claims and other information from jwtSecurityToken.Claims
            return jwtSecurityToken.ToString();
        }


        return null;
    }





    private static RSACryptoServiceProvider SetupRsaParameters(string privateKey)
    {
        // Remove the header and footer from the private key string
        var privateKeyFormatted = privateKey
            .Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");


        // Convert base64 encoded private key to bytes
        var privateKeyBytes = Convert.FromBase64String(privateKeyFormatted);

        // Import the private key bytes
        var rsa = new RSACryptoServiceProvider();

        rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
        return rsa;
    }
}