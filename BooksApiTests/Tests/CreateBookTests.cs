using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using BooksApiTests.Models;
using NUnit.Framework;

namespace BooksApiTests.Tests;

[AllureSuite("Books")]
[AllureFeature("Create Book")]
public class CreateBookTests : BaseTest
{
    [Test]
    [AllureDescription("Verify that a valid book is successfully created with 201 status")]
    [AllureSeverity(SeverityLevel.critical)]
    public void CreateBook_WithValidData_Returns201AndMatchingBody()
    {
        var bookRequest = BuildBookRequest(
            title: "Clean Code",
            author: "Robert C. Martin",
            publishedDate: new DateTime(2008, 8, 1),
            isAvailable: true
        );

        var response = apiClient.CreateBook(bookRequest);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(201),
                "Status code should be 201 Created");
            Assert.That(response.Data, Is.Not.Null, "Response body should not be null");
        });

        var createdBook = response.Data!;
        createdBookIds.Add(createdBook.id);

        Assert.Multiple(() =>
        {
            Assert.That(createdBook.id, Is.Not.EqualTo(Guid.Empty), "Book id should be a valid GUID");
            Assert.That(createdBook.title, Is.EqualTo(bookRequest.title), "Title should match");
            Assert.That(createdBook.author, Is.EqualTo(bookRequest.author), "Author should match");
            Assert.That(createdBook.publishedDate.Date, Is.EqualTo(bookRequest.publishedDate.Date),
                "PublishedDate should match");
        });
    }

    [Test]
    [AllureDescription("Verify that all required fields are present in the create book response")]
    [AllureSeverity(SeverityLevel.normal)]
    public void CreateBook_ResponseContainsAllFields()
    {
        var bookRequest = BuildBookRequest(
            title: "The Pragmatic Programmer",
            author: "Andrew Hunt",
            publishedDate: new DateTime(1999, 10, 20),
            isAvailable: false
        );

        var response = apiClient.CreateBook(bookRequest);
        var createdBook = response.Data!;
        createdBookIds.Add(createdBook.id);

        Assert.Multiple(() =>
        {
            Assert.That(createdBook.id, Is.Not.EqualTo(Guid.Empty), "id field should be present");
            Assert.That(createdBook.title, Is.Not.Empty, "title field should be present");
            Assert.That(createdBook.author, Is.Not.Empty, "author field should be present");
            Assert.That(createdBook.publishedDate, Is.Not.EqualTo(default(DateTime)),
                "publishedDate field should be present");
        });
    }

    [Test]
    [AllureDescription("Verify that duplicate book creation is handled correctly")]
    [AllureSeverity(SeverityLevel.normal)]
    public void CreateBook_DuplicateBook_ReturnsExpectedStatusCode()
    {
        var bookRequest = BuildBookRequest(title: "Duplicate Book Test");

        var firstResponse = apiClient.CreateBook(bookRequest);
        Assert.That((int)firstResponse.StatusCode, Is.EqualTo(201));
        createdBookIds.Add(firstResponse.Data!.id);

        var secondResponse = apiClient.CreateBook(bookRequest);

        var secondStatus = (int)secondResponse.StatusCode;
        Assert.That(secondStatus, Is.AnyOf(201, 409),
            $"Duplicate creation should return 201 or 409, but got {secondStatus}");

        if (secondStatus == 201 && secondResponse.Data is not null)
        {
            createdBookIds.Add(secondResponse.Data.id);
        }
            
    }
}