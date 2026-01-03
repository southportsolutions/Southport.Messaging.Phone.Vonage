using System.Net.Http;

namespace Southport.Messaging.Phone.Vonage.TextMessage;

public interface IVonageTextMessageFactory
{
    public VonageTextMessage Create();

    VonageTextMessage Create(string apiKey, string secret,
        string privateKey, string applicationId, int validFor, bool useMessageApi = false, bool useSandbox = false,
        string testPhoneNumbers = null);
}