using System;
using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared.Vonage;

public class VonageWebhookResponse
{
    [JsonProperty("to")] public string To { get; set; }

    [JsonProperty("from")] public string From { get; set; }

    [JsonProperty("channel")] public string Channel { get; set; }

    [JsonProperty("message_uuid")] public Guid MessageUuid { get; set; }

    [JsonProperty("timestamp")] public DateTimeOffset Timestamp { get; set; }

    [JsonProperty("usage")] public VonageUsage Usage { get; set; }

    [JsonProperty("message_type")] public string MessageType { get; set; }

    [JsonProperty("text")] public string Text { get; set; }

    [JsonProperty("sms")] public VonageSms Sms { get; set; }
}