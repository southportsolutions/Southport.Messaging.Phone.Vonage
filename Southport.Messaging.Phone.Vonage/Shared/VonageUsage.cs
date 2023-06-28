using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared;

public class VonageUsage
{
    [JsonProperty("price")] public string Price { get; set; }

    [JsonProperty("currency")] public string Currency { get; set; }
}