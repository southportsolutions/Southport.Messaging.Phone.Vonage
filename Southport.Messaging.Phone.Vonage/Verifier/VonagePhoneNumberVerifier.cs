using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Southport.Messaging.Phone.Core.Verifier;
using Southport.Messaging.Phone.Vonage.Shared.Options;
using Southport.Messaging.Phone.Vonage.Shared.Verify;
using Southport.Messaging.Phone.Vonage.Shared.Vonage;

namespace Southport.Messaging.Phone.Vonage.Verifier;

public class VonageVonagePhoneNumberVerifier : VonageClientBase, IVonagePhoneNumberVerifier
{
    public VonageVonagePhoneNumberVerifier(string apiKey, HttpClient httpClient, string secret, bool useSandbox, string privateKey, string applicationId, int validFor, string testPhoneNumbers) : base(httpClient, apiKey, secret, useSandbox, privateKey, applicationId, validFor, testPhoneNumbers)
    {
    }

    public VonageVonagePhoneNumberVerifier(HttpClient httpClient, IOptions<VonageOptions> options) : base(httpClient, options.Value)
    {
    }


    public async Task<VerifyResponse> PhoneNumberLookupAsync(string phoneNumber, PhoneNumberLookupType type, string countryCode)
    {
        VerifyResponse response = null;
        switch (type)
        {
            case PhoneNumberLookupType.Validate:
                var request = new VerifyRequest
                {
                    Number = phoneNumber,
                    Country = countryCode
                };
                // TODO
                //response = await InnerClient.VerifyClient.VerifyRequestAsync(request); //TODO
                break;
            case PhoneNumberLookupType.Carrier:
                break;
            case PhoneNumberLookupType.CallerName:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        return response;

    }

}