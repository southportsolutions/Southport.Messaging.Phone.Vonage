using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared.Verify;

public class VerifyResponse : VerifyResponseBase
{
    [JsonProperty("request_id")]
    public string RequestId { get; set; }
}