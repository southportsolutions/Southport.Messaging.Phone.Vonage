﻿using System;
using System.Collections.Generic;
using System.Linq;
using Southport.Messaging.Phone.Core.Response;
using Southport.Messaging.Phone.Core.Shared;
using Southport.Messaging.Phone.Vonage.Shared.SmsApi;

namespace Southport.Messaging.Phone.Vonage.TextMessage.Response
{
    public class VonageTextMessageResponse : ITextMessageResponse
    {

        public static explicit operator VonageTextMessageResponse(SendSmsResponse b) => new(b);

        VonageTextMessageResponse(SendSmsResponse messageResource)
        {
            double messageCost = 0;
            foreach (var message in messageResource.Messages)
            {
                double.TryParse(message.MessagePrice, out var cost);
                messageCost += cost;
            }
            Body = null;
            NumSegments = messageResource.MessageCount;
            Direction = messageResource.Direction;
            From = messageResource.From;
            To = PhoneHelper.NormalizePhoneNumber(messageResource.Messages.Select(e => e.To).FirstOrDefault());
            DateUpdated = null;
            Price = messageCost.ToString();
            ErrorMessage = string.Join("\n", messageResource.Messages.Select(e=>e.ErrorText));
            Uri = null;
            AccountSid = null;
            NumMedia = null;
            Status = string.Join("\n", messageResource.Messages.Select(e=>e.Status));
            MessagingServiceSid = null;
            Sid = messageResource.Messages.Select(e=>e.MessageId).FirstOrDefault();
            DateSent = null;
            DateCreated = null;
            ErrorCode = null;
            PriceUnit = null;
            ApiVersion = null;
            SubresourceUris = null;
            IsSuccessful = string.IsNullOrWhiteSpace(ErrorMessage);
        }

        private VonageTextMessageResponse(string message, string moreInfo, int errorCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
            MoreInfo = moreInfo;
            IsSuccessful = false;
        }

        public static ITextMessageResponse Failed(string message, string moreInfo, int errorCode)
        {
            return new VonageTextMessageResponse(message, moreInfo, errorCode);
        }

        public string Body { get; }
        public string NumSegments { get; }
        public DirectionEnum Direction { get; }
        public string From { get; }
        public string To { get; }
        public DateTime? DateUpdated { get; }
        public string Price { get; }
        public bool IsSuccessful { get; }
        public string ErrorMessage { get; }
        public string Uri { get; }
        public string AccountSid { get; }
        public string NumMedia { get; }
        public StatusEnum Status { get; }
        public string MessagingServiceSid { get; }
        public string Sid { get; }
        public DateTime? DateSent { get; }
        public DateTime? DateCreated { get; }
        public int? ErrorCode { get; }
        public string MoreInfo { get; }
        public string PriceUnit { get; }
        public string ApiVersion { get; }
        public Dictionary<string, string> SubresourceUris { get; }
    }
}
