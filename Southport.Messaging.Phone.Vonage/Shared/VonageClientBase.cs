using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Southport.Messaging.Phone.Core.Shared;

namespace Southport.Messaging.Phone.Vonage.Shared;

public abstract class VonageClientBase
{
        
    public bool UseSandbox { get; }
    public List<string> TestPhoneNumbers { get; } = new();

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly string _apiUrl = "https://rest.nexmo.com/sms/json";


    protected VonageClientBase(HttpClient httpClient, string apiKey, string secret, bool useSandbox,
        string testPhoneNumbers = null)
    {
        _apiKey = apiKey;
        _apiSecret = secret;
        _httpClient = httpClient;

        UseSandbox = useSandbox;


        if (!string.IsNullOrWhiteSpace(testPhoneNumbers))
        {
            TestPhoneNumbers.AddRange(testPhoneNumbers.Split(','));
        }

    }

    protected async Task<SendSmsResponse> SendSms(string from, string to, string message)
    {
        var collection = new List<KeyValuePair<string, string>>
        {
            new("text", message),
            new("from", PhoneHelper.NormalizePhoneNumber(from)),
            new("to", PhoneHelper.NormalizePhoneNumber(to)),
            new("api_key", _apiKey),
            new("api_secret", _apiSecret)
        };

        var content = new FormUrlEncodedContent(collection);

        var response = await _httpClient.PostAsync(_apiUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();
        var smsResponse = JsonConvert.DeserializeObject<SendSmsResponse>(responseString);

        var errorMessages = smsResponse.Messages.Where(e => e.StatusCode != SmsStatusCode.Success).ToList();
        if (!response.IsSuccessStatusCode || errorMessages.Any())
        {
            throw new VonageSmsResponseException(errorMessages.FirstOrDefault()?.ErrorText)
            {
                Response = smsResponse
            };
        }

        return smsResponse;
    }

    protected VonageClientBase(HttpClient httpClient, IVonageOptions options) : this(httpClient, options.ApiKey, options.Secret, options.UseSandbox, options.TestPhoneNumbers)
    {
    }
}