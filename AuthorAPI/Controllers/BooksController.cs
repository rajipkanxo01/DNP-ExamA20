using AuthorAPI.Domain;
using AuthorAPI.EFCDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly AuthorContext authorContext;

    public BooksController(AuthorContext authorContext)
    {
        this.authorContext = authorContext;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Book>>> GetAllBooksAsync()
    {
        try
        {
            List<Book> allBooks = await authorContext.Books!.ToListAsync();
            return allBooks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("{authorId:int}")]
    public async Task<ActionResult<Book>> CreateAsync([FromRoute] int authorId, [FromBody] Book book)
    {
        try
        {
            if (book.Title!.Length > 40 || string.IsNullOrEmpty(book.Title))
                throw new Exception("Invalid Book. Title should be at less than 40 characters!!!!");

            Author? author = await authorContext.Authors!.FindAsync(authorId);
            EntityEntry<Book> createdBook = await authorContext.Books!.AddAsync(book);
            
            author!.WrittenBooks!.Add(book);
            await authorContext.SaveChangesAsync();
            return Created($"/authors/{authorId}/{createdBook.Entity.Isbn}", createdBook.Entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            Book? findBook = await authorContext.Books!.FindAsync(id);

            if (findBook is null) throw new Exception($"Book with {id} could not be found");

            authorContext.Books.Remove(findBook);
            await authorContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}