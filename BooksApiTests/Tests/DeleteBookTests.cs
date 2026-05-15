using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace BooksApiTests.Tests;

[AllureSuite("Books")]
[AllureFeature("Delete Book")]
public class DeleteBookTests : BaseTest
{
    [Test]
    [AllureDescription("Verify deleting a book by valid ID returns 204 No Content")]
    [AllureSeverity(SeverityLevel.critical)]
    public void DeleteBook_ValidId_Returns204()
    {
        var createdBook = CreateBookAndTrack(BuildBookRequest(title: "Book To Delete"));
        createdBookIds.Remove(createdBook.id);

        var response = apiClient.DeleteBook(createdBook.id);

        Assert.That((int)response.StatusCode, Is.EqualTo(204),
            "Status code should be 204 No Content");
    }

    [Test]
    [AllureDescription("Verify deleted book is no longer retrievable - returns 404")]
    [AllureSeverity(SeverityLevel.critical)]
    public void DeleteBook_DeletedBookIsNotRetrievable()
    {
        var createdBook = CreateBookAndTrack(BuildBookRequest(title: "Book To Be Gone"));
        createdBookIds.Remove(createdBook.id);

        apiClient.DeleteBook(createdBook.id);

        var getResponse = apiClient.GetBookByIdRaw(createdBook.id.ToString());

        Assert.That((int)getResponse.StatusCode, Is.EqualTo(404),
            "Deleted book should return 404 when fetched");
    }

    [Test]
    [AllureDescription("Verify deleting a non-existent book ID returns 404")]
    [AllureSeverity(SeverityLevel.normal)]
    public void DeleteBook_NonExistentId_Returns404()
    {
        var nonExistentId = Guid.NewGuid();

        var response = apiClient.DeleteBookRaw(nonExistentId.ToString());

        Assert.That((int)response.StatusCode, Is.EqualTo(404),
            "Non-existent book ID should return 404 Not Found");
    }

    [Test]
    [AllureDescription("Verify deleting with invalid ID format returns 400")]
    [AllureSeverity(SeverityLevel.normal)]
    [TestCase("not-a-guid")]
    [TestCase("999")]
    [TestCase("@@@")]
    public void DeleteBook_InvalidIdFormat_Returns400(string invalidId)
    {
        var response = apiClient.DeleteBookRaw(invalidId);

        Assert.That((int)response.StatusCode, Is.EqualTo(400),
            $"Invalid ID format '{invalidId}' should return 400 Bad Request");
    }
}
