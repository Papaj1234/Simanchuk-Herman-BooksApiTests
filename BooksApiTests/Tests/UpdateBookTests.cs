using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using BooksApiTests.Models;
using NUnit.Framework;

namespace BooksApiTests.Tests;

[AllureSuite("Books")]
[AllureFeature("Update Book")]
public class UpdateBookTests : BaseTest
{
    [Test]
    [AllureDescription("Verify updating an existing book with valid data returns 204 and data is persisted")]
    [AllureSeverity(SeverityLevel.critical)]
    public void UpdateBook_WithValidData_ReturnsUpdatedBook()
    {
        var createdBook = CreateBookAndTrack(BuildBookRequest(
            title: "Original Title",
            author: "Original Author",
            isAvailable: true
        ));

        var updateRequest = new BookRequest
        {
            title = "Updated Title",
            author = "Updated Author",
            isbn = "978-1111111111",
            publishedDate = new DateTime(2023, 6, 15),
            isAvailable = false
        };

        var response = apiClient.UpdateBookRaw(createdBook.id.ToString(), updateRequest);

        Assert.That((int)response.StatusCode, Is.EqualTo(204),
            "Status code should be 204 No Content");

        var getResponse = apiClient.GetBookById(createdBook.id);
        Assert.That(getResponse.Data, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(getResponse.Data!.title, Is.EqualTo(updateRequest.title), "title should be updated");
            Assert.That(getResponse.Data!.author, Is.EqualTo(updateRequest.author), "author should be updated");
            Assert.That(getResponse.Data!.isAvailable, Is.EqualTo(updateRequest.isAvailable), "isAvailable should be updated");
        });
    }

    [Test]
    [AllureDescription("Verify that updated data is persisted - GET after PUT returns new values")]
    [AllureSeverity(SeverityLevel.normal)]
    public void UpdateBook_UpdatedDataIsPersisted()
    {
        var createdBook = CreateBookAndTrack(BuildBookRequest(title: "Before Update"));

        var updateRequest = BuildBookRequest(title: "After Update", isAvailable: false);
        apiClient.UpdateBook(createdBook.id, updateRequest);

        var getResponse = apiClient.GetBookById(createdBook.id);

        Assert.That(getResponse.Data, Is.Not.Null);
        Assert.That(getResponse.Data!.title, Is.EqualTo("After Update"),
            "Updated title should be persisted after GET");
    }

    [Test]
    [AllureDescription("Verify updating a non-existent book ID returns 404")]
    [AllureSeverity(SeverityLevel.normal)]
    public void UpdateBook_NonExistentId_Returns404()
    {
        var nonExistentId = Guid.NewGuid();
        var updateRequest = BuildBookRequest(title: "Ghost Book");

        var response = apiClient.UpdateBookRaw(nonExistentId.ToString(), updateRequest);

        Assert.That((int)response.StatusCode, Is.EqualTo(404),
            "Non-existent book ID should return 404 Not Found");
    }

    [Test]
    [AllureDescription("Verify updating with invalid ID format returns 400")]
    [AllureSeverity(SeverityLevel.normal)]
    [TestCase("not-a-guid")]
    [TestCase("abc")]
    public void UpdateBook_InvalidIdFormat_Returns400(string invalidId)
    {
        var updateRequest = BuildBookRequest(title: "Some Book");

        var response = apiClient.UpdateBookRaw(invalidId, updateRequest);

        Assert.That((int)response.StatusCode, Is.EqualTo(400),
            $"Invalid ID format '{invalidId}' should return 400 Bad Request");
    }
}