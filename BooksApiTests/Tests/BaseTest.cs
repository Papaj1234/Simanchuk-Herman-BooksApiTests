using Allure.NUnit;
using BooksApiTests.Helpers;
using BooksApiTests.Models;
using NUnit.Framework;

namespace BooksApiTests.Tests;

[AllureNUnit]
public abstract class BaseTest
{
    protected BooksApiClient apiClient = null!;
    protected readonly List<Guid> createdBookIds = new();

    [SetUp]
    public void SetUp()
    {
        apiClient = new BooksApiClient();
        TestLogger.Log($"=== Starting test: {TestContext.CurrentContext.Test.Name} ===");
    }

    [TearDown]
    public void TearDown()
    {
        TestLogger.Log($"=== Cleaning up after: {TestContext.CurrentContext.Test.Name} ===");

        foreach (var bookId in createdBookIds)
        {
            try
            {
                var response = apiClient.DeleteBook(bookId);
                TestLogger.Log($"Cleanup - deleted book {bookId}, status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                TestLogger.Log($"Cleanup warning - failed to delete book {bookId}: {ex.Message}");
            }
        }

        createdBookIds.Clear();
        TestLogger.Log($"=== Test finished: {TestContext.CurrentContext.Result.Outcome} ===");
    }

    protected BookRequest BuildBookRequest(
        string title = "Test Book",
        string author = "Test Author",
        string isbn = "978-0000000000",
        DateTime? publishedDate = null,
        bool isAvailable = true)
    {
        return new BookRequest
        {
            title = title,
            author = author,
            isbn = isbn,
            publishedDate = publishedDate ?? new DateTime(2020, 1, 1),
            isAvailable = isAvailable
        };
    }

    protected BookResponse CreateBookAndTrack(BookRequest? request = null)
    {
        var bookRequest = request ?? BuildBookRequest();
        var response = apiClient.CreateBook(bookRequest);

        Assert.That((int)response.StatusCode, Is.EqualTo(201),
            $"Expected 201 Created but got {(int)response.StatusCode}");
        Assert.That(response.Data, Is.Not.Null, "Response body should not be null");

        createdBookIds.Add(response.Data!.id);
        return response.Data;
    }
}