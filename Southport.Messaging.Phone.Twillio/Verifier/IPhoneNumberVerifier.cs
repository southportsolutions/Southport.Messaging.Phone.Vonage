using System.Threading.Tasks;
using Vonage.Verify;

namespace Southport.Messaging.Phone.Vonage.Verifier
{
    public interface IPhoneNumberVerifier
    {
        Task<VerifyResponse> PhoneNumberLookupAsync(string phoneNumber, PhoneNumberLookupType type, string countryCode);
    }
}