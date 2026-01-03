using System;
using Microsoft.Extensions.Configuration;
using Southport.Messaging.Phone.Vonage.Shared.Options;
using Southport.Messaging.Phone.Vonage.TextMessage;
using Southport.Messaging.Phone.Vonage.Verifier;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SouthportTwilioServiceCollection
{
    public static IServiceCollection AddKeyVaultCollection(this IServiceCollection services, IConfiguration config,
        string configKey = VonageOptions.Key)
    {
        ArgumentNullException.ThrowIfNull(configKey);

        if (string.IsNullOrWhiteSpace(configKey))
        {
            throw new ArgumentException("Configuration key cannot be empty or whitespace.", nameof(configKey));
        }

        services.Configure<VonageOptions>(config.GetSection(configKey));
        services.AddHttpClient<IVonageTextMessageFactory, VonageTextMessageFactory>();
        services.AddHttpClient<IVonagePhoneNumberVerifier, VonageVonagePhoneNumberVerifier>();

        return services;
    }
}