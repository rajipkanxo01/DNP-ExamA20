using AuthorAPI.Domain;

namespace AuthorBlazor.Services.ServiceInterface;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> CreateAsync(Book book, int authorId);
    Task DeleteAsync(int bookId);
}