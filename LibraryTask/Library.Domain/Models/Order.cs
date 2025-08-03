namespace Library.Domain.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    
    // navigation properties 
    public List<Book> Books { get; set; }
}