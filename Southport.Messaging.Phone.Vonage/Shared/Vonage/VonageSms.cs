using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared.Vonage;

public class VonageSms
{
    [JsonProperty("num_messages")]
    public long NumMessages { get; set; }

    [JsonProperty("count_total")]
    public long CountTotal { get; set; }
}