using System;
using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared;

public class MessageStatusWebhook
{
    [JsonProperty("message_uuid")]
    public Guid MessageUuid { get; set; }

    [JsonProperty("to")]
    public string To { get; set; }

    [JsonProperty("from")]
    public string From { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("error")]
    public MessageStatusWebhookError Error { get; set; }

    [JsonProperty("usage")]
    public VonageUsage Usage { get; set; }

    [JsonProperty("client_ref")]
    public string ClientRef { get; set; }

    [JsonProperty("channel")]
    public string Channel { get; set; }

    [JsonProperty("destination")]
    public MessageStatusWebhookDestination Destination { get; set; }

    [JsonProperty("sms")]
    public MessageStatusWebhookSms Sms { get; set; }
}

public class MessageStatusWebhookDestination
{
    [JsonProperty("network_code")]
    public string NetworkCode { get; set; }
}

public class MessageStatusWebhookError
{
    [JsonProperty("type")]
    public Uri Type { get; set; }

    [JsonProperty("title")]
    public int Title { get; set; }

    [JsonProperty("detail")]
    public string Detail { get; set; }

    [JsonProperty("instance")]
    public string Instance { get; set; }
}

public class MessageStatusWebhookSms
{
    [JsonProperty("count_total")]
    public int CountTotal { get; set; }
}