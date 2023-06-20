using System.Collections.Generic;

namespace Southport.Messaging.Phone.Vonage.Shared
{
    public interface ITwilioWebhook
    {
        string MessageSid { get; set; }
        string AccountSid { get; set; }
        string MessagingServiceSid { get; set; }
        string From { get; set; }
        string To { get; set; }
        string Body { get; set; }
        int NumMedia { get; set; } 
        List<string> MediaContentTypes { get; set; }
        List<string> MediaUrls { get; set; }
        string MessageStatus { get; set; }

    }

    public class TwilioWebhook : ITwilioWebhook
    {
        public TwilioWebhook()
        {
            MediaContentTypes = new List<string>();
            MediaUrls = new List<string>();
        }
        public string MessageSid { get; set; }
        public string AccountSid { get; set; }
        public string MessagingServiceSid { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public int NumMedia { get; set; }
        public List<string> MediaContentTypes { get; set; }
        public List<string> MediaUrls { get; set; }
        public string MessageStatus { get; set; }
    }
}