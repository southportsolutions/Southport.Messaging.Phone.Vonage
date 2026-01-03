using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared;

public class SendMessageResponse
{
    [JsonProperty("message_uuid")]
    public string MessageUuid { get; set; }
}