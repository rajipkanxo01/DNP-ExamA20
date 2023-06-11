using AuthorAPI.Domain;
using AuthorAPI.EFCDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AuthorContext authorContext;

    public AuthorsController(AuthorContext authorContext)
    {
        this.authorContext = authorContext;
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAsync(Author author)
    {
        try
        {
            if (author.FistName!.Length > 15 || string.IsNullOrEmpty(author.FistName))
                throw new Exception("Invalid First Name. Must be less than 15 characters");
            if (author.LastName!.Length > 15 || string.IsNullOrEmpty(author.LastName))
                throw new Exception("Invalid Last Name. Must be less than characters");

            EntityEntry<Author> createdAuthor = await authorContext.Authors!.AddAsync(author);
            await authorContext.SaveChangesAsync();
            return Created($"/authors/{createdAuthor.Entity.Id}", createdAuthor.Entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    /*[HttpGet]
    public async Task<ActionResult<ICollection<Author>>> GetAllAuthorsAsync()
    {
        try
        {
            List<Author> authors = await authorContext.Authors!.Include(author => author.WrittenBooks).ToListAsync();
            return authors;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }*/

    [HttpGet]
    public async Task<ActionResult<ICollection<Author>>> GetAllAuthorsAsync([FromQuery] string? firstName,
        [FromQuery] string? titleContains)
    {
        try
        {
            List<Author> authors = await authorContext.Authors!.Include(author => author.WrittenBooks).ToListAsync();
            if (!string.IsNullOrEmpty(firstName))
            {
                authors = authors.Where(a => a.FistName.Equals(firstName)).ToList();
            }

            if (!string.IsNullOrEmpty(titleContains))
            {
                foreach (var author in authors)
                {
                    List<Book> books = author.WrittenBooks!.Where(a => a.Title!.Contains(titleContains, StringComparison.OrdinalIgnoreCase)).ToList();
                    author.WrittenBooks = books;
                }
            }

            return authors;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}