namespace BooksApiTests.Helpers;

using NUnit.Framework;

public static class TestLogger
{
    public static void Log(string message)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] {message}");
        TestContext.Out.WriteLine($"[{timestamp}] {message}");
    }

    public static void LogRequest(string method, string url, string? body = null)
    {
        Log($"REQUEST: {method} {url}");
        if (!string.IsNullOrEmpty(body))
        {
            Log($"BODY: {body}");
        }
    }

    public static void LogResponse(int statusCode, string? body = null)
    {
        Log($"RESPONSE STATUS: {statusCode}");
        if (!string.IsNullOrEmpty(body))
        {
            Log($"RESPONSE BODY: {body}");
        }
    }
}
