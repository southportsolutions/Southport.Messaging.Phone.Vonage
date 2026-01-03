using System.Net.Http;
using Microsoft.Extensions.Options;
using Southport.Messaging.Phone.Vonage.Shared;

namespace Southport.Messaging.Phone.Vonage.TextMessage;

public class VonageTextMessageFactory : IVonageTextMessageFactory
{
    private readonly HttpClient _httpClient;
    private readonly VonageOptions _options;

    public VonageTextMessageFactory(HttpClient httpClient, IOptions<VonageOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public VonageTextMessage Create()
    {
        return new VonageTextMessage(_httpClient, _options);
    }

    public VonageTextMessage Create(string apiKey, string secret,
        string privateKey, string applicationId, int validFor, bool useMessageApi = false, bool useSandbox = false,
        string testPhoneNumbers = null)
    {
        return new VonageTextMessage(_httpClient, apiKey, secret, privateKey, applicationId, validFor, useMessageApi,
            useSandbox, testPhoneNumbers);
    }
}