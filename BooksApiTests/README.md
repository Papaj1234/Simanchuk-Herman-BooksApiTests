# BooksApiTests

Automated API tests for the Books API using **NUnit** + **RestSharp** + **Allure**.

## Project Structure

```
BooksApiTests/
├── Config/
│   └── AppSettings.cs          # Config model
├── Helpers/
│   ├── AuthHelper.cs           # OAuth2 client_credentials token
│   ├── BooksApiClient.cs       # RestSharp API wrapper
│   ├── ConfigurationHelper.cs  # Loads appsettings.json
│   └── TestLogger.cs           # Console + NUnit output logging
├── Models/
│   └── BookModels.cs           # BookRequest, BookResponse, AuthTokenResponse
├── Tests/
│   ├── BaseTest.cs             # SetUp / TearDown + helpers
│   ├── CreateBookTests.cs      # POST /api/books
│   ├── GetAllBooksTests.cs     # GET /api/books
│   ├── GetBookByIdTests.cs     # GET /api/books/{id}
│   ├── UpdateBookTests.cs      # PUT /api/books/{id}
│   └── DeleteBookTests.cs      # DELETE /api/books/{id}
├── appsettings.json            # Base URL + auth credentials
├── allureConfig.json           # Allure output folder
└── BooksApiTests.csproj
```

## Prerequisites

- .NET 8 SDK
- (Optional) Allure CLI for HTML reports

## Running Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ClassName=CreateBookTests"

# Run with verbose output
dotnet test -v normal
```

## Allure Reports

```bash
# Run tests (results go to allure-results/)
dotnet test

# Generate and open HTML report
allure serve allure-results
```

## Configuration

Edit `appsettings.json` to change the base URL or auth credentials:

```json
{
  "Api": {
    "baseUrl": "https://lecture-books-api.azurewebsites.net/api",
    "authUrl": "https://login.microsoftonline.com/.../oauth2/v2.0/token"
  },
  "Auth": {
    "clientId": "...",
    "clientSecret": "...",
    "scope": "...",
    "grantType": "client_credentials"
  }
}
```

## Test Coverage

| Scenario | Test Cases |
|---|---|
| Create Book | Valid creation (201), field validation, duplicate handling |
| Get All Books | 200 + non-null list, all fields present, created book visible |
| Get Book By ID | Valid ID (200), non-existent (404), invalid format (400) |
| Update Book | Valid update (200), data persisted, non-existent (404), invalid (400) |
| Delete Book | Valid delete (204), not retrievable after delete, non-existent (404), invalid (400) |
