using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared.MessageApi;

public class SendMessageResponse
{
    [JsonProperty("message_uuid")]
    public string MessageUuid { get; set; }
}