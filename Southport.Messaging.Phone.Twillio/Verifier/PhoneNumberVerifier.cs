using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Southport.Messaging.Phone.Vonage.Shared;
using Vonage.Common;
using Vonage.Verify;

namespace Southport.Messaging.Phone.Vonage.Verifier;

public class PhoneNumberVerifier : VonageClientBase, IPhoneNumberVerifier
{
    public PhoneNumberVerifier(string accountSid, string apiKey, string secret, bool useSandbox) : base(apiKey, secret, useSandbox)
    {
    }

    public PhoneNumberVerifier(IVonageOptions options) : base(options)
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

public enum PhoneNumberLookupType
{
    Validate,
    Carrier,
    CallerName
}