using AuthorAPI.Domain;

namespace AuthorBlazor.Services.ServiceInterface;

public interface IAuthorService
{
    Task<Author> CreateAsync(Author author);

    // Task<IEnumerable<Author>> GetAuthors();
    Task<IEnumerable<Author>> GetAuthorsAsync(string? authorFirstNameFilter = null, string? titleContainsFilter = null);
}