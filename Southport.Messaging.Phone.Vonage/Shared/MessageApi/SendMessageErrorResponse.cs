using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared.MessageApi;

public class SendMessageErrorResponse
{
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("detail")] public string Detail { get; set; }
    [JsonProperty("instance")] public string Instance { get; set; }
}