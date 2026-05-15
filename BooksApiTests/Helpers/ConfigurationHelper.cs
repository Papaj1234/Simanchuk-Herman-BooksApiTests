using BooksApiTests.Config;
using Microsoft.Extensions.Configuration;

namespace BooksApiTests.Helpers;

public static class ConfigurationHelper
{
    private static AppSettings? _settings;

    public static AppSettings GetSettings()
    {
        if (_settings is not null)
        {
            return _settings;
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        _settings = new AppSettings();
        _settings.Api = configuration.GetSection("Api").Get<ApiSettings>() ?? new();
        _settings.Auth = configuration.GetSection("Auth").Get<AuthSettings>() ?? new();

        return _settings;
    }
}