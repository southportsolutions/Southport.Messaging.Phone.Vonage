namespace Southport.Messaging.Phone.Vonage.Shared;

public interface IVonageOptions
{
    string TestPhoneNumbers { get; set; }
    bool UseSandbox { get; set; }

    #region Basic Auth
    string ApiKey { get; set; }
    string Secret { get; set; }
    #endregion

    #region JWT Auth

    string PrivateKey { get; set; }
    string ApplicationId { get; set; }
    int ValidFor { get; set; }

    #endregion

    bool UseMessageApi { get; set; }

}