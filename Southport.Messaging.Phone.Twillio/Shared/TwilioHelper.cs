using System.Linq;
using System.Web;

namespace Southport.Messaging.Phone.Vonage.Shared
{
    public static class TwilioHelper
    {
        public static string NormalizePhoneNumber(string phoneNumber)
        {
            var startWithPlus = phoneNumber.StartsWith("+");
            if (startWithPlus)
            {
                return phoneNumber;
            }
            
            if (phoneNumber.StartsWith("1") == false)
            {
                return $"+1{phoneNumber}";
                
            }

            return $"+{phoneNumber}";
        }

        public static ITwilioWebhook ParseWebhook(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            var array = message.Split('&');
            var parameterDictionary = array.Select(item => item.Split('=')).ToDictionary(keyValue => keyValue[0], keyValue => HttpUtility.UrlDecode(keyValue[1]));

            var webhook = new TwilioWebhook()
            {
                MessageSid = parameterDictionary["MessageSid"],
                AccountSid = parameterDictionary["AccountSid"],
                MessagingServiceSid = parameterDictionary.ContainsKey("MessagingServiceSid") ? parameterDictionary["MessagingServiceSid"] : null,
                From = parameterDictionary["From"],
                To = parameterDictionary["To"],
                Body = parameterDictionary["Body"],
                MessageStatus = parameterDictionary.ContainsKey("MessageStatus") ? parameterDictionary["MessageStatus"] : null
            };

            if (int.TryParse(parameterDictionary["NumMedia"], out var numMedia))
            {
                webhook.NumMedia = numMedia;
            }

            for (var i = 0; i < numMedia; i++)
            {
                webhook.MediaContentTypes.Add(parameterDictionary[$"MediaContentType{i}"]);
                webhook.MediaUrls.Add(parameterDictionary[$"MediaUrl{i}"]);
            }

            return webhook;
        }
    }
}
