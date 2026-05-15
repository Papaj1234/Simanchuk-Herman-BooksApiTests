using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace BooksApiTests.Tests;

[AllureSuite("Books")]
[AllureFeature("Get All Books")]
public class GetAllBooksTests : BaseTest
{
    [Test]
    [AllureDescription("Verify the API returns a list of books with 200 status")]
    [AllureSeverity(SeverityLevel.critical)]
    public void GetAllBooks_Returns200AndNonNullList()
    {
        var response = apiClient.GetAllBooks();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200),
                "Status code should be 200 OK");
            Assert.That(response.Data, Is.Not.Null, "Response body should not be null");
        });
    }

    [Test]
    [AllureDescription("Verify each book in the list contains all required fields")]
    [AllureSeverity(SeverityLevel.normal)]
    public void GetAllBooks_EachBookContainsRequiredFields()
    {
        // Ensure at least one book exists
        var createdBook = CreateBookAndTrack(BuildBookRequest(title: "Field Validation Book"));

        var response = apiClient.GetAllBooks();
        Assert.That(response.Data, Is.Not.Null);

        var books = response.Data!;
        Assert.That(books, Is.Not.Empty, "Books list should not be empty");

        foreach (var book in books)
        {
            Assert.Multiple(() =>
            {
                Assert.That(book.id, Is.Not.EqualTo(Guid.Empty),
                    $"Book should have a valid id, but got empty GUID");
                Assert.That(book.title, Is.Not.Null,
                    $"Book {book.id} should have a title");
                Assert.That(book.author, Is.Not.Null,
                    $"Book {book.id} should have an author");
                Assert.That(book.publishedDate, Is.Not.EqualTo(default(DateTime)),
                    $"Book {book.id} should have a publishedDate");
            });
        }
    }

    [Test]
    [AllureDescription("Verify a newly created book appears in the list")]
    [AllureSeverity(SeverityLevel.normal)]
    public void GetAllBooks_RecentlyCreatedBookIsPresent()
    {
        var createdBook = CreateBookAndTrack(
            BuildBookRequest(title: "Book For List Check")
        );

        var response = apiClient.GetAllBooks();
        Assert.That(response.Data, Is.Not.Null);

        var bookIds = response.Data!.Select(b => b.id).ToList();
        Assert.That(bookIds, Does.Contain(createdBook.id),
            "Newly created book should appear in the all books list");
    }
}
