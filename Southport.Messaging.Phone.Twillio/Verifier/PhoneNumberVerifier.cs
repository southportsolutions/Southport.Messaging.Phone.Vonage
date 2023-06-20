using System.Collections.Generic;
using System.Threading.Tasks;
using Southport.Messaging.Phone.Vonage.Shared;
using Twilio.Rest.Lookups.V1;
using Twilio.Types;
using HttpClient = System.Net.Http.HttpClient;

namespace Southport.Messaging.Phone.Vonage.Verifier
{
    public class PhoneNumberVerifier : TwilioClientBase, IPhoneNumberVerifier
    {
        public PhoneNumberVerifier(HttpClient httpClient, string accountSid, string apiKey, string authToken, bool useSandbox) : base(httpClient, accountSid, apiKey, authToken, useSandbox)
        {
        }

        public PhoneNumberVerifier(HttpClient httpClient, ITwilioOptions options) : base(httpClient, options)
        {
        }


        public async Task<PhoneNumberResource> PhoneNumberLookupAsync(string phoneNumber, PhoneNumberLookupType type, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrEmpty(phoneNumber))
            {
                return null;
            }

            phoneNumber = TwilioHelper.NormalizePhoneNumber(phoneNumber);
            var types = new List<string>();
            switch (type)
            {
                case PhoneNumberLookupType.Carrier:
                    types.Add("carrier");
                    break;
                case PhoneNumberLookupType.CallerName:
                    types.Add("caller-name");
                    break;
                default:
                    types = null;
                    break;

            }

            return await PhoneNumberResource.FetchAsync(type: types, countryCode: countryCode, pathPhoneNumber: new PhoneNumber(phoneNumber), client: _innerClient);
        }

    }

    public enum PhoneNumberLookupType
    {
        Validate,
        Carrier,
        CallerName
    }
}
