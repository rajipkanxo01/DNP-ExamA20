using System.ComponentModel.DataAnnotations;

namespace AuthorAPI.Domain;

public class Author
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(15)] public string? FistName { get; set; }
    [Required, MaxLength(15)] public string? LastName { get; set; }
    public ICollection<Book>? WrittenBooks { get; set; }


    public Author(string? fistName, string? lastName)
    {
        FistName = fistName;
        LastName = lastName;
        WrittenBooks = new List<Book>();
    }
}