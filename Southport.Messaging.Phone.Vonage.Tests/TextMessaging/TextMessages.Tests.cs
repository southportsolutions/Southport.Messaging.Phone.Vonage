using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Southport.Messaging.Phone.Core.TextMessage;
using Southport.Messaging.Phone.Vonage.Shared.MessageApi;
using Southport.Messaging.Phone.Vonage.Shared.Options;
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
            var fromNumber = "badfromNumber";
            var message = "test\n\r";

            var mockHttp = new MockHttpMessageHandler();

            var responseData = new SendMessageErrorResponse
            {
                Type = "https://developer.nexmo.com/api-errors/messages-olympus#1120",
                Title = "Invalid sender",
                Detail = "The `from` parameter is invalid for the given channel.",
                Instance = "ae1c0b1a-6f7d-4f9a-8a7d-0c295e7e4d7c"
            };
            var responseContent = new StringContent(JsonConvert.SerializeObject(responseData), Encoding.UTF8, "application/json");
            mockHttp.When(HttpMethod.Post, "*").Respond(HttpStatusCode.UnprocessableEntity, responseContent);
            var textMessage = new VonageTextMessage(mockHttp.ToHttpClient(), Options.ApiKey, Options.Secret, Options.PrivateKey, Options.ApplicationId, Options.ValidFor, true, false, Options.TestPhoneNumbers);

            var response = await textMessage
                .SetFrom(fromNumber)
                .SetTo(toNumber)
                .SetMessage(message)
                .SendAsync();

            var expectedErrorCode = 422;

            Assert.False(response.IsSuccessful);
            Assert.Equal(expectedErrorCode, response.ErrorCode);
            Assert.Equal(responseData.Title, response.ErrorMessage);
            Assert.Equal(responseData.Detail, response.MoreInfo);
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
