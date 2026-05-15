using Newtonsoft.Json;

namespace BooksApiTests.Models;

public class BookRequest
{
    [JsonProperty("title")]
    public string title { get; set; } = string.Empty;

    [JsonProperty("author")]
    public string author { get; set; } = string.Empty;

    [JsonProperty("isbn")]
    public string isbn { get; set; } = string.Empty;

    [JsonProperty("publishedDate")]
    public DateTime publishedDate { get; set; }

    [JsonProperty("isAvailable")]
    public bool isAvailable { get; set; }
}

public class BookResponse
{
    [JsonProperty("id")]
    public Guid id { get; set; }

    [JsonProperty("title")]
    public string title { get; set; } = string.Empty;

    [JsonProperty("author")]
    public string author { get; set; } = string.Empty;

    [JsonProperty("isbn")]
    public string isbn { get; set; } = string.Empty;

    [JsonProperty("publishedDate")]
    public DateTime publishedDate { get; set; }

    [JsonProperty("isAvailable")]
    public bool isAvailable { get; set; }
}

public class AuthTokenResponse
{
    [JsonProperty("access_token")]
    public string accessToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string tokenType { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int expiresIn { get; set; }
}