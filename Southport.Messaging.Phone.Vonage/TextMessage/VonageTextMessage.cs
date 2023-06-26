using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Southport.Messaging.Phone.Core.Response;
using Southport.Messaging.Phone.Core.Shared;
using Southport.Messaging.Phone.Core.TextMessage;
using Southport.Messaging.Phone.Vonage.Shared;
using Southport.Messaging.Phone.Vonage.TextMessage.Response;
using Vonage.Messaging;


namespace Southport.Messaging.Phone.Vonage.TextMessage
{
    public class VonageTextMessage : VonageClientBase, ITextMessage
    {

        public VonageTextMessage(IVonageOptions options) : base(options)
        {
        }

        public VonageTextMessage(string apiKey, string secret, bool useSandbox = false, string testPhoneNumbers = null) : base(apiKey, secret, useSandbox, testPhoneNumbers)
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
            From = PhoneHelper.NormalizePhoneNumber(from);;
            return this;
        }

        public ITextMessage SetTo(string to)
        {
            To = PhoneHelper.NormalizePhoneNumber(to);
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
                //var messageResponse = await MessageResource.CreateAsync(
                //    new PhoneNumber(To),
                //    from: new PhoneNumber(from),
                //    body: Message,
                //    messagingServiceSid: MessageServiceSid,
                //    client: InnerClient); // pass in the custom client

                var response = await InnerClient.SmsClient.SendAnSmsAsync(new SendSmsRequest()
                {
                    To = To,
                    From = From,
                    Text = Message
                });

                return (VonageTextMessageResponse) response;
            }
            catch (VonageSmsResponseException e) // todo correct exception
            {
                int.TryParse(e.Response.Messages.FirstOrDefault()?.Status, out var statusCode);
                return VonageTextMessageResponse.Failed(e.Message, e.Response.Messages.FirstOrDefault()?.StatusCode.ToString(), statusCode);
            }

        }

        private async Task<ITextMessageResponse> SendTestPhoneNumbersAsync(string from)
        {
            VonageTextMessageResponse messageResponse = null;
            foreach (var to in TestPhoneNumbers.Select(PhoneHelper.NormalizePhoneNumber))
            {
                messageResponse = (VonageTextMessageResponse)await InnerClient.SmsClient.SendAnSmsAsync(new SendSmsRequest
                {
                    To = To,
                    From = From,
                    Text = Message
                });
            }


            return messageResponse;
        }
    }
}