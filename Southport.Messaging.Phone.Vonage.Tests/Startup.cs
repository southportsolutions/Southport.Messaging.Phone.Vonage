using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Southport.Messaging.Phone.Vonage.Shared.Options;

namespace Southport.Messaging.Phone.Vonage.Tests
{
    public static class Startup
    {
        private static VonageOptionsTest OptionsTest { get; set; }
        public static VonageOptionsTest GetOptions()
        {
            if (OptionsTest == null)
            {
                var configurationBuilder = new ConfigurationBuilder()
                    
                    .AddJsonFile(Path.Combine((new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent).ToString(), "appsettings.json"), true)
                    .AddEnvironmentVariables();
                var config = configurationBuilder.Build();
                OptionsTest = new VonageOptionsTest { UseSandbox = false};
                config.Bind(OptionsTest);

                if (string.IsNullOrWhiteSpace(OptionsTest.Secret))
                {
                    OptionsTest.ApiKey = Environment.GetEnvironmentVariable("VONAGE_API_KEY");
                    OptionsTest.Secret = Environment.GetEnvironmentVariable("VONAGE_SECRET");
                    OptionsTest.PrivateKey = Environment.GetEnvironmentVariable("VONAGE_PRIVATE_KEY");
                    OptionsTest.ApplicationId = Environment.GetEnvironmentVariable("VONAGE_APPLICATION_ID");
                    OptionsTest.From = Environment.GetEnvironmentVariable("VONAGE_FROM");
                    OptionsTest.To = Environment.GetEnvironmentVariable("VONAGE_TO");
                }

                if (string.IsNullOrEmpty(OptionsTest.ApiKey))
                {
                    throw new Exception("Unable to get the Vonage API Key.");
                }
            }

            return OptionsTest;

        }
    }

    public class VonageOptionsTest : VonageOptions
    {
        public string To { get; set; }
        public string From { get; set; }
    }
}