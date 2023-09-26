using Southport.Messaging.Phone.Vonage.Shared.MessageApi;
using Southport.Messaging.Phone.Vonage.Shared.SmsApi;

namespace Southport.Messaging.Phone.Vonage.Shared.Vonage;

public class VonageSmsResponseException : VonageException
{
    public VonageSmsResponseException(string message, SendSmsResponse response) : base(message)
    {
        Response = response;
    }

    public SendSmsResponse Response { get; private set; }
}

public class VonageMessageResponseException : VonageException
{
    public VonageMessageResponseException(string message, SendMessageErrorResponse response, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
        Response = response;
    }

    public int ErrorCode { get; private set; }
    public SendMessageErrorResponse Response { get; private set; }
}