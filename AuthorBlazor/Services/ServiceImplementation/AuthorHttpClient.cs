using System.Text.Json;
using AuthorAPI.Domain;
using AuthorBlazor.Services.ServiceInterface;

namespace AuthorBlazor.Services.ServiceImplementation;

public class AuthorHttpClient : IAuthorService
{
    private readonly HttpClient client;

    public AuthorHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<Author> CreateAsync(Author author)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/authors", author);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        Author createdAuthor = JsonSerializer.Deserialize<Author>(result,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return createdAuthor;
    }

    public async Task<IEnumerable<Author>> GetAuthorsAsync(string? authorFirstNameFilter,
        string? titleContainsFilter)
    {
        string query = ConstructQuery(authorFirstNameFilter, titleContainsFilter);

        HttpResponseMessage responseMessage = await client.GetAsync("/Authors" + query);
        string content = await responseMessage.Content.ReadAsStringAsync();


        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode}, {content}");
        }

        ICollection<Author> allAuthors = JsonSerializer.Deserialize<ICollection<Author>>(content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return allAuthors;
    }

    private string ConstructQuery(string? authorFirstNameFilter, string? titleContainsFilter)
    {
        string query = "";
        if (!string.IsNullOrEmpty(authorFirstNameFilter))
        {
            query += $"?firstName={authorFirstNameFilter}";
        }

        if (!string.IsNullOrEmpty(titleContainsFilter))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"titlecontains={titleContainsFilter}";
        }

        return query;
    }


    /*public async Task<IEnumerable<Author>> GetAuthors()
    {
        HttpResponseMessage response = await client.GetAsync("/authors");
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        ICollection<Author> authors = JsonSerializer.Deserialize<ICollection<Author>>(result,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return authors;
    }*/
}