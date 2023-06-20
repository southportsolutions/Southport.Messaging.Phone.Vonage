using System;
using System.Collections.Generic;
using Twilio.Types;

namespace Southport.Messaging.Phone.Vonage.TextMessage.Response
{
    public interface ITextMessageResponse
    {
        /// <summary>The message text</summary>
        string Body { get; }

        /// <summary>The number of messages used to deliver the message body</summary>
        string NumSegments { get; }

        /// <summary>The direction of the message</summary>
        DirectionEnum Direction { get; }

        /// <summary>The phone number that initiated the message</summary>
        PhoneNumber From { get; }

        /// <summary>The phone number that received the message</summary>
        string To { get; }

        /// <summary>
        /// The RFC 2822 date and time in GMT that the resource was last updated
        /// </summary>
        DateTime? DateUpdated { get; }

        /// <summary>The amount billed for the message</summary>
        string Price { get; }

        /// <summary>
        /// The URI of the resource, relative to `https://api.twilio.com`
        /// </summary>
        string Uri { get; }

        /// <summary>The SID of the Account that created the resource</summary>
        string AccountSid { get; }

        /// <summary>The number of media files associated with the message</summary>
        string NumMedia { get; }

        /// <summary>The status of the message</summary>
        StatusEnum Status { get; }

        /// <summary>The SID of the Messaging Service used with the message.</summary>
        string MessagingServiceSid { get; }

        /// <summary>The unique string that identifies the resource</summary>
        string Sid { get; }

        /// <summary>
        /// The RFC 2822 date and time in GMT when the message was sent
        /// </summary>
        DateTime? DateSent { get; }

        /// <summary>
        /// The RFC 2822 date and time in GMT that the resource was created
        /// </summary>
        DateTime? DateCreated { get; }

        /// <summary>The currency in which price is measured</summary>
        string PriceUnit { get; }

        /// <summary>The API version used to process the message</summary>
        string ApiVersion { get; }

        /// <summary>
        /// A list of related resources identified by their relative URIs
        /// </summary>
        Dictionary<string, string> SubresourceUris { get; }

        bool IsSuccessful { get; }

        /// <summary>The description of the error_code</summary>
        string ErrorMessage { get; }

        /// <summary>The error code associated with the message</summary>
        int? ErrorCode { get; }

        string MoreInfo { get; }

    }
}