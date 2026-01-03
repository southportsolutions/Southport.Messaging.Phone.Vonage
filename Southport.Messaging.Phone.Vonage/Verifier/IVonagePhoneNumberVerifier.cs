using System.Threading.Tasks;
using Southport.Messaging.Phone.Core.Verifier;
using Southport.Messaging.Phone.Vonage.Shared.Verify;

namespace Southport.Messaging.Phone.Vonage.Verifier
{
    public interface IVonagePhoneNumberVerifier
    {
        Task<VerifyResponse> PhoneNumberLookupAsync(string phoneNumber, PhoneNumberLookupType type, string countryCode);
    }
}
