using Southport.Messaging.Phone.Vonage.Shared;

namespace Southport.Messaging.Phone.Vonage.TextMessage.Response
{
    public sealed class StatusEnum : StringEnum
    {
        public static readonly StatusEnum Queued = new StatusEnum("queued");
        public static readonly StatusEnum Sending = new StatusEnum("sending");
        public static readonly StatusEnum Sent = new StatusEnum("sent");
        public static readonly StatusEnum Failed = new StatusEnum("failed");
        public static readonly StatusEnum Delivered = new StatusEnum("delivered");
        public static readonly StatusEnum Undelivered = new StatusEnum("undelivered");
        public static readonly StatusEnum Receiving = new StatusEnum("receiving");
        public static readonly StatusEnum Received = new StatusEnum("received");
        public static readonly StatusEnum Accepted = new StatusEnum("accepted");
        public static readonly StatusEnum Scheduled = new StatusEnum("scheduled");
        public static readonly StatusEnum Read = new StatusEnum("read");
        public static readonly StatusEnum PartiallyDelivered = new StatusEnum("partially_delivered");

        private StatusEnum(string value)
            : base(value)
        {
        }

        public StatusEnum()
        {
        }

        public static implicit operator StatusEnum(string value) => new StatusEnum(value);
    }
}