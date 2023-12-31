﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Southport.Messaging.Phone.Core.Response;
using Southport.Messaging.Phone.Core.Shared;
using Southport.Messaging.Phone.Core.TextMessage;
using Southport.Messaging.Phone.Vonage.Shared.Options;
using Southport.Messaging.Phone.Vonage.Shared.SmsApi;
using Southport.Messaging.Phone.Vonage.Shared.Vonage;
using Southport.Messaging.Phone.Vonage.TextMessage.Response;


namespace Southport.Messaging.Phone.Vonage.TextMessage
{
    public class VonageTextMessage : VonageClientBase, ITextMessage
    {
        private readonly bool _useMessageApi;

        public VonageTextMessage(HttpClient httpClient, IVonageOptions options) : base(httpClient, options)
        {
            _useMessageApi = options.UseMessageApi;
        }

        public VonageTextMessage(HttpClient httpClient, string apiKey, string secret,
            string privateKey, string applicationId, int validFor, bool useMessageApi = false, bool useSandbox = false,
            string testPhoneNumbers = null) : base(httpClient, apiKey, secret, useSandbox, privateKey, applicationId, validFor, testPhoneNumbers)
        {
            _useMessageApi = useMessageApi;
        }

        private readonly List<string> _testFromNumbers = new()
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
            From = PhoneHelper.NormalizePhoneNumber(from);
            return this;
        }

        public ITextMessage SetTo(string to)
        {
            to = Regex.Replace(to, @"[^a-zA-Z0-9]", "");
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
                throw new VonageException("The Message cannot be null or empty.");
            }
            
            var from = UseSandbox && !_testFromNumbers.Contains(From) ? "+15005550006" : From;

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new VonageException("The From phone number cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(To))
            {
                throw new VonageException("The To phone number cannot be null or empty.");
            }

            if (TestPhoneNumbers.Any())
            {
                return await SendTestPhoneNumbersAsync(from);
            }

            try
            {
                SendSmsResponse response;
                if (_useMessageApi)
                {
                    response = await SendMessage(from, To, Message);
                }
                else
                {
                    response = await SendSms(from, To, Message);
                }

                return ProcessResponse(response);
            }
            catch (VonageSmsResponseException e)
            {
                var message = e.Response.Messages.FirstOrDefault();
                int.TryParse(message?.Status, out var statusCode);
                return VonageTextMessageResponse.Failed(e.Message, message?.StatusCode.ToString(), statusCode);
            }
            catch (VonageMessageResponseException e)
            {
                return VonageTextMessageResponse.Failed(e.Response?.Title, e.Response?.Detail, e.ErrorCode);
            }

        }

        private async Task<ITextMessageResponse> SendTestPhoneNumbersAsync(string from)
        {
            SendSmsResponse messageResponse = null;
            foreach (var to in TestPhoneNumbers.Select(PhoneHelper.NormalizePhoneNumber))
            {
                messageResponse = await SendSms(from, to, Message);
            }


            return ProcessResponse(messageResponse);
        }

        private ITextMessageResponse ProcessResponse(SendSmsResponse response)
        {
            response.From = From;
            response.Direction = DirectionEnum.OutboundApi;
            return (VonageTextMessageResponse)response;

        }

    }
}