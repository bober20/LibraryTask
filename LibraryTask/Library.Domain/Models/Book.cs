namespace Library.Domain.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    
    // navigation properties 
    public Order Order { get; set; }
}