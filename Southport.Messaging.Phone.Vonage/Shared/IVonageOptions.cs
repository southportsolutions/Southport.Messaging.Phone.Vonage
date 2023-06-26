namespace Southport.Messaging.Phone.Vonage.Shared;

public interface IVonageOptions
{
    string TestPhoneNumbers { get; set; }
    bool UseSandbox { get; set; }
    string ApiKey { get; set; }
    string Secret { get; set; }

}