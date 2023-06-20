using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Southport.Messaging.Phone.Vonage.Shared;
using Southport.Messaging.Phone.Vonage.TextMessage.Response;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using HttpClient = System.Net.Http.HttpClient;

namespace Southport.Messaging.Phone.Vonage.TextMessage
{
    public class TextMessage : TwilioClientBase, ITextMessage
    {
        public TextMessage(HttpClient httpClient, ITwilioOptions options) : base(httpClient, options)
        {
            MessageServiceSid = options.MessagingServiceSid;
        }

        public TextMessage(HttpClient httpClient, string accountSid, string apiKey, string authToken, bool useSandbox = false, string testPhoneNumbers = null) : base(httpClient, accountSid, apiKey, authToken, useSandbox, testPhoneNumbers)
        {
        }

        private List<string> _testFromNumbers = new List<string>()
        {
            "+15005550001",
            "+15005550007",
            "+15005550008",
            "+15005550006"
        };


        public string From { get; set; }
        public string To { get; set; }
        public string MessageServiceSid { get; set; }
        public string Message { get; set; }
        public ITextMessage SetFrom(string from)
        {
            From = TwilioHelper.NormalizePhoneNumber(from);;
            return this;
        }

        public ITextMessage SetTo(string to)
        {
            To = TwilioHelper.NormalizePhoneNumber(to);
            return this;
        }

        public ITextMessage SetMessageServicesSid(string messageServiceSid)
        {
            MessageServiceSid = messageServiceSid;
            return this;
        }

        public ITextMessage SetMessage(string message)
        {
            Message = message;
            return this;
        }

        public async Task<ITextMessageResponse> SendAsync()
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                throw new NullReferenceException("The Message cannot be null or empty.");
            }
            
            var from = UseSandbox && _testFromNumbers.Contains(From) == false ? "+15005550006" : From;

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new NullReferenceException("The From phone number cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(To))
            {
                throw new NullReferenceException("The To phone number cannot be null or empty.");
            }

            if (TestPhoneNumbers.Any())
            {
                return await SendTestPhoneNumbersAsync(from);
            }

            try
            {
                var messageResponse = await MessageResource.CreateAsync(
                    new PhoneNumber(To),
                    from: new PhoneNumber(from),
                    body: Message,
                    messagingServiceSid: MessageServiceSid,
                    client: _innerClient); // pass in the custom client

                return (TextMessageResponse) messageResponse;
            }
            catch (ApiException e)
            {
                return TextMessageResponse.Failed(e.Message, e.MoreInfo, e.Code);
            }

        }

        private async Task<ITextMessageResponse> SendTestPhoneNumbersAsync(string from)
        {
            TextMessageResponse messageResponse = null;
            foreach (var to in TestPhoneNumbers.Select(TwilioHelper.NormalizePhoneNumber))
            {
                var twilioResponse = await MessageResource.CreateAsync(
                    new PhoneNumber(to),
                    from: new PhoneNumber(from),
                    body: Message,
                    messagingServiceSid: MessageServiceSid ?? MessageServiceSid,
                    client: _innerClient); // pass in the custom client

                messageResponse = (TextMessageResponse) twilioResponse;
            }


            return messageResponse;
        }
    }
}
