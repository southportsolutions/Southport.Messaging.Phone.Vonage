namespace Southport.Messaging.Phone.Vonage.Shared.Options;

public class VonageOptions
{
    public const string Key = "Vonage";
    
    public string TestPhoneNumbers { get; set; }
    public bool UseSandbox { get; set; }

    #region Basic Auth
    public string ApiKey { get; set; }
    public string Secret { get; set; }
    #endregion

    #region JWT Auth

    public string PrivateKey { get; set; }
    public string ApplicationId { get; set; }
    public int ValidFor { get; set; }

    #endregion

    public bool UseMessageApi { get; set; }

}