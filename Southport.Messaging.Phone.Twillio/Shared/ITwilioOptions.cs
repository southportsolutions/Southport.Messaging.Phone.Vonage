namespace Southport.Messaging.Phone.Vonage.Shared;

public interface ITwilioOptions
{
    string AccountSid { get; set; }
    string ApiKey { get; set; }
    string AuthToken { get; set; }
    bool UseSandbox { get; set; }
    string TestPhoneNumbers { get; set; }
    string MessagingServiceSid { get; set; }
}

public interface IVonageOptions
{
    string TestPhoneNumbers { get; set; }
    bool UseSandbox { get; set; }
    string ApiKey { get; set; }
    string Secret { get; set; }

}