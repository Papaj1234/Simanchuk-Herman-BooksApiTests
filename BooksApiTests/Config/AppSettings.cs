namespace BooksApiTests.Config;

public class AppSettings
{
    public ApiSettings Api { get; set; } = new();
    public AuthSettings Auth { get; set; } = new();
}

public class ApiSettings
{
    public string baseUrl { get; set; } = string.Empty;
    public string authUrl { get; set; } = string.Empty;
}

public class AuthSettings
{
    public string clientId { get; set; } = string.Empty;
    public string clientSecret { get; set; } = string.Empty;
    public string scope { get; set; } = string.Empty;
    public string grantType { get; set; } = string.Empty;
}
