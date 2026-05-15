using BooksApiTests.Models;
using Newtonsoft.Json;
using RestSharp;

namespace BooksApiTests.Helpers;

public static class AuthHelper
{
    private static string? _cachedToken;
    private static DateTime _tokenExpiry = DateTime.MinValue;

    public static string GetBearerToken()
    {
        if (_cachedToken is not null && DateTime.UtcNow < _tokenExpiry)
        {
            return _cachedToken;
        }

        var settings = ConfigurationHelper.GetSettings();
        var client = new RestClient(settings.Api.authUrl);
        var request = new RestRequest(string.Empty, Method.Post);

        request.AddParameter("client_Id", settings.Auth.clientId);
        request.AddParameter("client_Secret", settings.Auth.clientSecret);
        request.AddParameter("scope", settings.Auth.scope);
        request.AddParameter("grant_type", settings.Auth.grantType);
        request.AddHeader("Accept", "application/json");

        var response = client.Execute(request);

        if (!response.IsSuccessful || response.Content is null)
        {
            throw new Exception($"Failed to get auth token. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
        }


        var tokenResponse = JsonConvert.DeserializeObject<AuthTokenResponse>(response.Content)
            ?? throw new Exception("Failed to deserialize auth token response");

        _cachedToken = tokenResponse.accessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.expiresIn - 60);

        TestLogger.Log($"Auth token acquired, expires in {tokenResponse.expiresIn}s");
        return _cachedToken;
    }
}
