using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Southport.Messaging.Phone.Core.TextMessage;
using Southport.Messaging.Phone.Vonage.TextMessage;
using Xunit;
using Xunit.Abstractions;

namespace Southport.Messaging.Phone.Vonage.Tests.TextMessaging
{
    public class TextMessageTests
    {
        private readonly ITestOutputHelper _output;
        private ITextMessage TextMessage { get; }
        private VonageOptionsTest OptionsTest { get; }
        public TextMessageTests(ITestOutputHelper output)
        {
            _output = output;
            OptionsTest = Startup.GetOptions();

            TextMessage = new VonageTextMessage(new HttpClient(), OptionsTest);
            TextMessage.MessageServiceSid = null;
        }

        [Fact]
        public async Task Send_BadFromNumber()
        {
            var toNumber = OptionsTest.To;
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
            var toNumber = OptionsTest.To;
            var fromNumber = OptionsTest.From;
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
