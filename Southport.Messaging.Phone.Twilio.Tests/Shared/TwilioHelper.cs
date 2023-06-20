using Southport.Messaging.Phone.Vonage.Shared;
using Xunit;

namespace Southport.Messaging.Phone.Vonage.Tests.Shared
{
    public class TwilioHelperTests
    {
        [Fact]
        public void PhoneNumberNormalize_NoStart1()
        {
            var phoneNumber = "2035559874";
            var normalizedPhoneNumber = TwilioHelper.NormalizePhoneNumber(phoneNumber);
            Assert.Equal($"+1{phoneNumber}", normalizedPhoneNumber);
        }
        [Fact]
        public void PhoneNumberNormalize_NoStartPlus()
        {
            var phoneNumber = "12035559874";
            var normalizedPhoneNumber = TwilioHelper.NormalizePhoneNumber(phoneNumber);
            Assert.Equal($"+{phoneNumber}", normalizedPhoneNumber);
        }
        [Fact]
        public void PhoneNumberNormalize_StartPlus()
        {
            var phoneNumber = "+12035559874";
            var normalizedPhoneNumber = TwilioHelper.NormalizePhoneNumber(phoneNumber);
            Assert.Equal($"{phoneNumber}", normalizedPhoneNumber);
        }

        [Fact]
        public void Webhook_Data_MMS_NoMessagingService_NoMessageStatus()
        {
            var accountSid = "ACe2cdfdaa687e260e98c3f81f7896ce97";
            var body = "Test mms";
            var from = "+12032572584";
            var to = "+19362373705";
            var mediaNumber = 1;
            var mediaContentType = "image/jpeg";
            var mediaUrl = "https://api.twilio.com/2010-04-01/Accounts/ACe2cdfdaa687e260e98c3f81f7896ce97/Messages/MM1c05a7205210e96eac11f4e1a484d3f2/Media/MEfb1d3ef549ca3899527125e69466bf8a";


            var data = "ToCountry=US&MediaContentType0=image%2Fjpeg&ToState=TX&SmsMessageSid=MM1c05a7205210e96eac11f4e1a484d3f2&NumMedia=1&ToCity=&FromZip=06607&SmsSid=MM1c05a7205210e96eac11f4e1a484d3f2&FromState=CT&SmsStatus=received&FromCity=BRIDGEPORT&Body=Test+mms&FromCountry=US&To=%2B19362373705&ToZip=&NumSegments=1&MessageSid=MM1c05a7205210e96eac11f4e1a484d3f2&AccountSid=ACe2cdfdaa687e260e98c3f81f7896ce97&From=%2B12032572584&MediaUrl0=https%3A%2F%2Fapi.twilio.com%2F2010-04-01%2FAccounts%2FACe2cdfdaa687e260e98c3f81f7896ce97%2FMessages%2FMM1c05a7205210e96eac11f4e1a484d3f2%2FMedia%2FMEfb1d3ef549ca3899527125e69466bf8a&ApiVersion=2010-04-01";
            var parsedData = TwilioHelper.ParseWebhook(data);

            Assert.Equal(accountSid, parsedData.AccountSid);
            Assert.Equal(body, parsedData.Body);
            Assert.Equal(from, parsedData.From);
            Assert.Equal(to, parsedData.To);
            Assert.Equal(mediaNumber, parsedData.NumMedia);
            Assert.Contains(mediaContentType, parsedData.MediaContentTypes);
            Assert.Contains(mediaUrl, parsedData.MediaUrls);
            
        }

        [Fact]
        public void Webhook_Data_MMS_MessagingService_MessageStatus()
        {
            var messagingServiceSid = "123456";
            var messageStatus = "delivered";
            var accountSid = "ACe2cdfdaa687e260e98c3f81f7896ce97";
            var body = "Test mms";
            var from = "+12032572584";
            var to = "+19362373705";
            var mediaNumber = 1;
            var mediaContentType = "image/jpeg";
            var mediaUrl = "https://api.twilio.com/2010-04-01/Accounts/ACe2cdfdaa687e260e98c3f81f7896ce97/Messages/MM1c05a7205210e96eac11f4e1a484d3f2/Media/MEfb1d3ef549ca3899527125e69466bf8a";


            var data = "MessagingServiceSid=123456&MessageStatus=delivered&ToCountry=US&MediaContentType0=image%2Fjpeg&ToState=TX&SmsMessageSid=MM1c05a7205210e96eac11f4e1a484d3f2&NumMedia=1&ToCity=&FromZip=06607&SmsSid=MM1c05a7205210e96eac11f4e1a484d3f2&FromState=CT&SmsStatus=received&FromCity=BRIDGEPORT&Body=Test+mms&FromCountry=US&To=%2B19362373705&ToZip=&NumSegments=1&MessageSid=MM1c05a7205210e96eac11f4e1a484d3f2&AccountSid=ACe2cdfdaa687e260e98c3f81f7896ce97&From=%2B12032572584&MediaUrl0=https%3A%2F%2Fapi.twilio.com%2F2010-04-01%2FAccounts%2FACe2cdfdaa687e260e98c3f81f7896ce97%2FMessages%2FMM1c05a7205210e96eac11f4e1a484d3f2%2FMedia%2FMEfb1d3ef549ca3899527125e69466bf8a&ApiVersion=2010-04-01";
            var parsedData = TwilioHelper.ParseWebhook(data);

            Assert.Equal(messagingServiceSid, parsedData.MessagingServiceSid);
            Assert.Equal(messageStatus, parsedData.MessageStatus);
            Assert.Equal(accountSid, parsedData.AccountSid);
            Assert.Equal(body, parsedData.Body);
            Assert.Equal(from, parsedData.From);
            Assert.Equal(to, parsedData.To);
            Assert.Equal(mediaNumber, parsedData.NumMedia);
            Assert.Contains(mediaContentType, parsedData.MediaContentTypes);
            Assert.Contains(mediaUrl, parsedData.MediaUrls);
            
        }
    }
}
