using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Southport.Messaging.Phone.Vonage.Shared;

namespace Southport.Messaging.Phone.Vonage.Tests
{
    public static class Startup
    {
        private static IVonageOptions Options { get; set; }
        public static IVonageOptions GetOptions()
        {
            if (Options == null)
            {
                var configurationBuilder = new ConfigurationBuilder()
                    
                    .AddJsonFile(Path.Combine((new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent).ToString(), "appsettings.json"), true)
                    .AddEnvironmentVariables();
                var config = configurationBuilder.Build();
                Options = new VonageOptions { UseSandbox = true};
                config.Bind(Options);

                if (string.IsNullOrWhiteSpace(Options.Secret))
                {
                    Options.Secret = Environment.GetEnvironmentVariable("VONAGE_SECRET");
                    Options.ApiKey = Environment.GetEnvironmentVariable("VOAGE_API_KEY");
                }

                if (string.IsNullOrEmpty(Options.ApiKey))
                {
                    throw new Exception("Unable to get the Vonage API Key.");
                }
            }

            return Options;

        }
    }

    public class VonageOptions : IVonageOptions
    {
        public string TestPhoneNumbers { get; set; }
        public bool UseSandbox { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
    }
}