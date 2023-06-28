using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Southport.Messaging.Phone.Core.TextMessage;
using Southport.Messaging.Phone.Vonage.Shared;
using Southport.Messaging.Phone.Vonage.TextMessage;
using Xunit;
using Xunit.Abstractions;

namespace Southport.Messaging.Phone.Vonage.Tests.TextMessaging
{
    public class TextMessageTests
    {
        private readonly ITestOutputHelper _output;
        private ITextMessage TextMessage { get; }
        private VonageOptions Options { get; }
        public TextMessageTests(ITestOutputHelper output)
        {
            _output = output;
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            Options = Startup.GetOptions();

            fixture.Register(()=>(IVonageOptions)Options);
            fixture.Register(()=> new HttpClient());

            TextMessage = fixture.Create<VonageTextMessage>();
            TextMessage.MessageServiceSid = null;
        }

        [Fact]
        public async Task Send_BadFromNumber()
        {
            var toNumber = Options.To;
            var fromNumber = "++17362271837";
            var message = "Testing";

            var response = await TextMessage
                .SetFrom(fromNumber)
                .SetTo(toNumber)
                .SetMessage(message)
                .SendAsync();

            var expectedErrorCode = 15;

            Assert.False(response.IsSuccessful);
            Assert.Equal(expectedErrorCode, response.ErrorCode);
            Assert.False(string.IsNullOrWhiteSpace(response.ErrorMessage));
            Assert.False(string.IsNullOrWhiteSpace(response.MoreInfo));
        }

        [Fact]
        public async Task Send_Success()
        {
            var toNumber = Options.To;
            var fromNumber = Options.From;
            var message = "Testing";

            var response = await TextMessage
                .SetFrom(fromNumber)
                .SetTo(toNumber)
                .SetMessage(message)
                .SendAsync();
            

            Assert.True(response.IsSuccessful);
        }
    }
}
