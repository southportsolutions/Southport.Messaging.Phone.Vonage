using System.Threading.Tasks;
using Southport.Messaging.Phone.Vonage.TextMessage.Response;

namespace Southport.Messaging.Phone.Vonage.TextMessage
{
    public interface ITextMessage
    {
        string From { get; set;  }
        string To { get; set; }
        string MessageServiceSid { get; set; }
        string Message { get; set; }

        ITextMessage SetFrom(string from);
        ITextMessage SetTo (string to);
        ITextMessage SetMessageServicesSid(string messageServiceSid);
        ITextMessage SetMessage(string message);
        Task<ITextMessageResponse> SendAsync();
    }
}