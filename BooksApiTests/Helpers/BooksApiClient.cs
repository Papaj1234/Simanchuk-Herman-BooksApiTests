using BooksApiTests.Models;
using Newtonsoft.Json;
using RestSharp;

namespace BooksApiTests.Helpers;

public class BooksApiClient
{
    private readonly RestClient _client;
    private readonly string _baseUrl;

    public BooksApiClient()
    {
        var settings = ConfigurationHelper.GetSettings();
        _baseUrl = settings.Api.baseUrl;
        _client = new RestClient(_baseUrl);
    }

    private RestRequest CreateRequest(string resource, Method method)
    {
        var request = new RestRequest(resource, method);
        request.AddHeader("Authorization", $"Bearer {AuthHelper.GetBearerToken()}");
        request.AddHeader("Content-Type", "application/json");
        return request;
    }

    public RestResponse<List<BookResponse>> GetAllBooks()
    {
        var request = CreateRequest("/Books", Method.Get);
        TestLogger.LogRequest("GET", $"{_baseUrl}/Books");
        var response = _client.Execute<List<BookResponse>>(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse<BookResponse> GetBookById(Guid bookId)
    {
        var request = CreateRequest($"/Books/{bookId}", Method.Get);
        TestLogger.LogRequest("GET", $"{_baseUrl}/Books/{bookId}");
        var response = _client.Execute<BookResponse>(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse GetBookByIdRaw(string bookId)
    {
        var request = CreateRequest($"/Books/{bookId}", Method.Get);
        TestLogger.LogRequest("GET", $"{_baseUrl}/Books/{bookId}");
        var response = _client.Execute(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse<BookResponse> CreateBook(BookRequest bookRequest)
    {
        var request = CreateRequest("/Books", Method.Post);
        var body = JsonConvert.SerializeObject(bookRequest);
        request.AddStringBody(body, DataFormat.Json);
        TestLogger.LogRequest("POST", $"{_baseUrl}/Books", body);
        var response = _client.Execute<BookResponse>(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse<BookResponse> UpdateBook(Guid bookId, BookRequest bookRequest)
    {
        var request = CreateRequest($"/Books/{bookId}", Method.Put);
        var body = JsonConvert.SerializeObject(bookRequest);
        request.AddStringBody(body, DataFormat.Json);
        TestLogger.LogRequest("PUT", $"{_baseUrl}/Books/{bookId}", body);
        var response = _client.Execute<BookResponse>(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse UpdateBookRaw(string bookId, BookRequest bookRequest)
    {
        var request = CreateRequest($"/Books/{bookId}", Method.Put);
        var body = JsonConvert.SerializeObject(bookRequest);
        request.AddStringBody(body, DataFormat.Json);
        TestLogger.LogRequest("PUT", $"{_baseUrl}/Books/{bookId}", body);
        var response = _client.Execute(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse DeleteBook(Guid bookId)
    {
        var request = CreateRequest($"/Books/{bookId}", Method.Delete);
        TestLogger.LogRequest("DELETE", $"{_baseUrl}/Books/{bookId}");
        var response = _client.Execute(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }

    public RestResponse DeleteBookRaw(string bookId)
    {
        var request = CreateRequest($"/Books/{bookId}", Method.Delete);
        TestLogger.LogRequest("DELETE", $"{_baseUrl}/Books/{bookId}");
        var response = _client.Execute(request);
        TestLogger.LogResponse((int)response.StatusCode, response.Content);
        return response;
    }
}