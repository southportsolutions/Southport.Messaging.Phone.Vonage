using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Jose;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

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

    public static string Generate(string privateKey, string applicationId, int expiresInSeconds = 1800, List<AclPath> accessControls = null)
    {
        var currentTime = DateTime.UtcNow - EpochTime;
        var currentTimeSeconds = currentTime.TotalSeconds;

        if (Tokens.ContainsKey(applicationId))
        {
            if (Tokens[applicationId].ExpiresAt > currentTimeSeconds + 60)
            {
                return Tokens[applicationId].Token;
            }
        }

        var acls = new Acls { Paths = accessControls ?? new List<AclPath> { Acls.Sessions, Acls.Conversations, Acls.Image } };
        var exp = currentTime.Add(TimeSpan.FromSeconds(expiresInSeconds));
        var expTotalSeconds = exp.TotalSeconds;
        var claims = new
        {
            application_id = applicationId,
            iat = currentTimeSeconds,
            exp = expTotalSeconds,
            jti = Guid.NewGuid(),
            acls
        };
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var json = JsonConvert.SerializeObject(claims, settings);

        using var rsa = SetupRsaParameters(privateKey);
        var jwt = JWT.Encode(json, rsa, algorithm: JwsAlgorithm.RS256);
        Tokens[applicationId] = new VonageToken(expTotalSeconds, jwt);
        return jwt;


        //accessControls ??= new List<AclPath> { Acls.Sessions, Acls.Conversations, Acls.Image };
        //var tokenHandler = new JwtSecurityTokenHandler();
        //var key = Encoding.ASCII.GetBytes(privateKey);
        //var aclValue = JsonConvert.SerializeObject(new Acls() { Paths = accessControls });
        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Expires = DateTime.UtcNow.AddSeconds(expiresInSeconds),
        //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        //    Claims = new Dictionary<string, object>
        //    {
        //        { "application_id", applicationId },
        //        { "acl", new Acls() { Paths = accessControls } }
        //    }
        //};
        //var token = tokenHandler.CreateToken(tokenDescriptor);
        //var tokenString = tokenHandler.WriteToken(token);
        //return tokenString;
    }

    public static string Decode(string jwt, string privateKey)
    {
        using var rsa = SetupRsaParameters(privateKey);
        return JWT.Decode(jwt, rsa, alg: JwsAlgorithm.RS256);
    }

    /// <summary>
    /// extra step at the end of construction to full initalize the RSA parameters.
    /// </summary>
    private static RSACryptoServiceProvider SetupRsaParameters(string privateKey)
    {
        using var sr = new StringReader(privateKey);
        var pemReader = new PemReader(sr);
        var kp = pemReader.ReadObject();
        var privateRsaParams = kp switch
        {
            null => throw new ArgumentException($"Invalid Private Key provided"),
            AsymmetricCipherKeyPair pair => pair.Private as RsaPrivateCrtKeyParameters,
            _ => kp as RsaPrivateCrtKeyParameters
        };
        var parameters = DotNetUtilities.ToRSAParameters(privateRsaParams);
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(parameters);
        return rsa;
    }
}

