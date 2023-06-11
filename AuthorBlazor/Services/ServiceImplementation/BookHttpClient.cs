using System.Text.Json;
using AuthorAPI.Domain;
using AuthorBlazor.Services.ServiceInterface;

namespace AuthorBlazor.Services.ServiceImplementation;

public class BookHttpClient : IBookService
{
    private readonly HttpClient client;

    public BookHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        HttpResponseMessage response = await client.GetAsync("/books");
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        ICollection<Book> books = JsonSerializer.Deserialize<ICollection<Book>>(result,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return books;
    }

    public async Task<Book> CreateAsync(Book book, int authorId)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"/books/{authorId}", book);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        Book user = JsonSerializer.Deserialize<Book>(result)!;
        return user;
    }

    public async Task DeleteAsync(int bookId)
    {
        HttpResponseMessage responseMessage = await client.DeleteAsync($"/books/{bookId}");
        if (!responseMessage.IsSuccessStatusCode)
        {
            string content = await responseMessage.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }
}