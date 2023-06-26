using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Southport.Messaging.Phone.Vonage.TextMessage;
using Xunit;
using Xunit.Abstractions;

namespace Southport.Messaging.Phone.Vonage.Tests.TextMessaging
{
    public class TextMessageTests
    {
        private readonly ITestOutputHelper _output;
        private ITextMessage TextMessage { get; }
        public TextMessageTests(ITestOutputHelper output)
        {
            _output = output;
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var options = Startup.GetOptions();

            fixture.Register(()=>options);
            fixture.Register(()=> new HttpClient());

            TextMessage = fixture.Create<TextMessage.TextMessage>();
            TextMessage.MessageServiceSid = null;
        }

        [Fact]
        public async Task Send_BadFromNumber()
        {
            var toNumber = "+16154610621";
            var fromNumber = "++1736271837";
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
            var toNumber = "+16154610621";
            var fromNumber = "+13023515152";
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
