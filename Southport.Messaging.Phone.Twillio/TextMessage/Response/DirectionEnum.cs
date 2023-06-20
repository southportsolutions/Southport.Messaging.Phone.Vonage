using Southport.Messaging.Phone.Vonage.Shared;

namespace Southport.Messaging.Phone.Vonage.TextMessage.Response
{
    public sealed class DirectionEnum : StringEnum
    {
        public static readonly DirectionEnum Inbound = new DirectionEnum("inbound");
        public static readonly DirectionEnum OutboundApi = new DirectionEnum("outbound-api");
        public static readonly DirectionEnum OutboundCall = new DirectionEnum("outbound-call");
        public static readonly DirectionEnum OutboundReply = new DirectionEnum("outbound-reply");

        private DirectionEnum(string value)
            : base(value)
        {
        }

        public static implicit operator DirectionEnum(string value) => new DirectionEnum(value);
    }
}