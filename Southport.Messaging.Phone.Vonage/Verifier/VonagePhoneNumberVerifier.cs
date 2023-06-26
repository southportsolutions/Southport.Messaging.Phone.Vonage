using System;
using System.Threading.Tasks;
using Southport.Messaging.Phone.Core.Verifier;
using Southport.Messaging.Phone.Vonage.Shared;
using Vonage.Verify;

namespace Southport.Messaging.Phone.Vonage.Verifier;

public class VonageVonagePhoneNumberVerifier : VonageClientBase, IVonagePhoneNumberVerifier
{
    public VonageVonagePhoneNumberVerifier(string accountSid, string apiKey, string secret, bool useSandbox) : base(apiKey, secret, useSandbox)
    {
    }

    public VonageVonagePhoneNumberVerifier(IVonageOptions options) : base(options)
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
                response = await InnerClient.VerifyClient.VerifyRequestAsync(request);
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