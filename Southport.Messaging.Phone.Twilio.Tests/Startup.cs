using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Southport.Messaging.Phone.Twilio.Shared;

namespace Southport.Messaging.Phone.Twilio.Tests
{
    public static class Startup
    {
        private static ITwilioOptions Options { get; set; }
        public static ITwilioOptions GetOptions()
        {
            if (Options == null)
            {
                var configurationBuilder = new ConfigurationBuilder()
                    
                    .AddJsonFile(Path.Combine((new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent).ToString(), "appsettings.json"), true)
                    .AddEnvironmentVariables();
                var config = configurationBuilder.Build();
                Options = new TwilioOptions {UseSandbox = true};
                config.Bind(Options);

                if (string.IsNullOrWhiteSpace(Options.AccountSid))
                {
                    Options.AccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                    Options.AuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
                }

                if (string.IsNullOrEmpty(Options.AccountSid))
                {
                    throw new Exception("Unable to get the Twilio API Key.");
                }
            }

            return Options;

        }
    }

    public class TwilioOptions : ITwilioOptions
    {
        public string AccountSid { get; set; }
        public string ApiKey { get; set; }
        public string AuthToken { get; set; }
        public bool UseSandbox { get; set; }
        public string TestPhoneNumbers { get; set; }
        public string MessagingServiceSid { get; set; }
    }
}