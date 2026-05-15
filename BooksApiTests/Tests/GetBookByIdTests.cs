using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace BooksApiTests.Tests;

[AllureSuite("Books")]
[AllureFeature("Get Book By ID")]
public class GetBookByIdTests : BaseTest
{
    [Test]
    [AllureDescription("Verify fetching a valid book by ID returns correct data with 200 status")]
    [AllureSeverity(SeverityLevel.critical)]
    public void GetBookById_ValidId_Returns200AndCorrectData()
    {
        var createdBook = CreateBookAndTrack(BuildBookRequest(
            title: "Domain-Driven Design",
            author: "Eric Evans",
            publishedDate: new DateTime(2003, 8, 30),
            isAvailable: true
        ));

        var response = apiClient.GetBookById(createdBook.id);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200),
                "Status code should be 200 OK");
            Assert.That(response.Data, Is.Not.Null, "Response body should not be null");
        });

        var fetchedBook = response.Data!;

        Assert.Multiple(() =>
        {
            Assert.That(fetchedBook.id, Is.EqualTo(createdBook.id), "id should match");
            Assert.That(fetchedBook.title, Is.EqualTo(createdBook.title), "title should match");
            Assert.That(fetchedBook.author, Is.EqualTo(createdBook.author), "author should match");
            Assert.That(fetchedBook.publishedDate.Date, Is.EqualTo(createdBook.publishedDate.Date),
                "publishedDate should match");
            Assert.That(fetchedBook.isAvailable, Is.EqualTo(createdBook.isAvailable),
                "isAvailable should match");
        });
    }

    [Test]
    [AllureDescription("Verify fetching a non-existent book ID returns 404")]
    [AllureSeverity(SeverityLevel.normal)]
    public void GetBookById_NonExistentId_Returns404()
    {
        var nonExistentId = Guid.NewGuid();

        var response = apiClient.GetBookByIdRaw(nonExistentId.ToString());

        Assert.That((int)response.StatusCode, Is.EqualTo(404),
            "Non-existent book ID should return 404 Not Found");
    }

    [Test]
    [AllureDescription("Verify fetching a book with invalid ID format returns 400")]
    [AllureSeverity(SeverityLevel.normal)]
    [TestCase("not-a-guid")]
    [TestCase("12345")]
    [TestCase("!!invalid!!")]
    public void GetBookById_InvalidIdFormat_Returns400(string invalidId)
    {
        var response = apiClient.GetBookByIdRaw(invalidId);

        Assert.That((int)response.StatusCode, Is.EqualTo(400),
            $"Invalid ID format '{invalidId}' should return 400 Bad Request");
    }
}
