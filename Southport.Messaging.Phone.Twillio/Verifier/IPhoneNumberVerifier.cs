using System.Threading.Tasks;
using Twilio.Rest.Lookups.V1;

namespace Southport.Messaging.Phone.Vonage.Verifier
{
    public interface IPhoneNumberVerifier
    {
        Task<PhoneNumberResource> PhoneNumberLookupAsync(string phoneNumber, PhoneNumberLookupType type, string countryCode);
    }
}