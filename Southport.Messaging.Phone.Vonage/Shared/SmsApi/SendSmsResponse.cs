using Newtonsoft.Json;
using Southport.Messaging.Phone.Core.Response;

namespace Southport.Messaging.Phone.Vonage.Shared.SmsApi;

public class SendSmsResponse
{
    /// <summary>
    /// The amount of messages in the request
    /// </summary>
    [JsonProperty("message-count")]
    public string MessageCount { get; set; }

    public SmsResponseMessage[] Messages { get; set; }
    public string From { get; set; }
    public DirectionEnum Direction { get; set; }
}