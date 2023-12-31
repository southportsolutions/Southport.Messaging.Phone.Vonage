﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Southport.Messaging.Phone.Core.Response;
using Southport.Messaging.Phone.Core.Shared;
using Southport.Messaging.Phone.Vonage.Shared.Jwt;
using Southport.Messaging.Phone.Vonage.Shared.MessageApi;
using Southport.Messaging.Phone.Vonage.Shared.Options;
using Southport.Messaging.Phone.Vonage.Shared.SmsApi;

namespace Southport.Messaging.Phone.Vonage.Shared.Vonage;

public abstract class VonageClientBase
{

    public bool UseSandbox { get; }
    public List<string> TestPhoneNumbers { get; } = new();

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly string _smsApiUrl = "https://rest.nexmo.com/sms/json";
    private readonly string _messagesApiUrl = "https://api.nexmo.com/v1/messages/";

    private readonly string _privateKey;
    private readonly string _applicationId;
    private readonly int _validFor;

    protected VonageClientBase(HttpClient httpClient, string apiKey, string secret, bool useSandbox, string privateKey,
        string applicationId, int validFor, string testPhoneNumbers = null)
    {
        _apiKey = apiKey;
        _apiSecret = secret;
        _httpClient = httpClient;

        UseSandbox = useSandbox;


        if (!string.IsNullOrWhiteSpace(testPhoneNumbers))
        {
            TestPhoneNumbers.AddRange(testPhoneNumbers.Split(','));
        }

        _privateKey = privateKey;
        _applicationId = applicationId;
        _validFor = validFor;
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

        var response = await _httpClient.PostAsync(_smsApiUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();
        var smsResponse = JsonConvert.DeserializeObject<SendSmsResponse>(responseString);

        var errorMessages = smsResponse.Messages.Where(e => e.StatusCode != SmsStatusCode.Success).ToList();
        if (!response.IsSuccessStatusCode || errorMessages.Any())
        {
            throw new VonageSmsResponseException(errorMessages.FirstOrDefault()?.ErrorText, smsResponse);
        }

        return smsResponse;
    }

    protected async Task<SendSmsResponse> SendMessage(string from, string to, string message)
    {
        var body = new
        {
            message_type = "text",
            text = message,
            to,
            from,
            channel = "sms"
        };


        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var jwtToken = JwtGenerator.Generate(_privateKey, _applicationId);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


        var response = await _httpClient.PostAsync(_messagesApiUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var responseError = JsonConvert.DeserializeObject<SendMessageErrorResponse>(responseString);
            throw new VonageMessageResponseException(response.ReasonPhrase, responseError, (int)response.StatusCode);
        }

        var smsResponse = JsonConvert.DeserializeObject<SendMessageResponse>(responseString);

        return new SendSmsResponse
        {
            From = from,
            Direction = DirectionEnum.OutboundApi,
            Messages = new List<SmsResponseMessage>()
            {
                new()
                {
                    MessageId = smsResponse.MessageUuid,
                    To = to,
                }
            }.ToArray()

        };
    }

    protected VonageClientBase(HttpClient httpClient, IVonageOptions options) : this(httpClient, options.ApiKey,
        options.Secret, options.UseSandbox, options.PrivateKey, options.ApplicationId, options.ValidFor,
        options.TestPhoneNumbers)
    {
    }
}