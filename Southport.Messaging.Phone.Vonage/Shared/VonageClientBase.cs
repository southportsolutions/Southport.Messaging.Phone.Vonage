using System.Collections.Generic;
using Vonage;
using Vonage.Request;

namespace Southport.Messaging.Phone.Vonage.Shared;

public abstract class VonageClientBase
{
    protected  readonly VonageClient InnerClient;
        
    public bool UseSandbox { get; }
    public List<string> TestPhoneNumbers { get; } = new();

    protected VonageClientBase(string apiKey, string secret, bool useSandbox, string testPhoneNumbers = null)
    {
        UseSandbox = useSandbox;

        InnerClient = new VonageClient(Credentials.FromApiKeyAndSecret(apiKey, secret));
            

        if (string.IsNullOrWhiteSpace(testPhoneNumbers) == false)
        {
            TestPhoneNumbers.AddRange(testPhoneNumbers.Split(','));
        }

        //if (UseSandbox)
        //{
        //    InnerClient = new TwilioRestClient(
        //        accountSid,
        //        authToken,
        //        httpClient: new SystemNetHttpClient(httpClient));
        //}
        //else
        //{
        //    InnerClient = new TwilioRestClient(
        //        apiKey,
        //        authToken,
        //        httpClient: new SystemNetHttpClient(httpClient),
        //        accountSid: accountSid);
        //}
    }

    protected VonageClientBase(IVonageOptions options) : this(options.ApiKey, options.Secret, options.UseSandbox, options.TestPhoneNumbers)
    {
    }
}