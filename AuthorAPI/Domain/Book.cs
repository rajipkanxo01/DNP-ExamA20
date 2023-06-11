using System.ComponentModel.DataAnnotations;

namespace AuthorAPI.Domain;

public class Book
{
    [Key]
    public int Isbn { get; set; }
    [Required,MaxLength(40)]
    public string? Title { get; set; }
    public int PublicationYear { get; set; }
    public int NumOfPages { get; set; }
    public string? Genre { get; set; }
}