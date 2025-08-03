namespace Library.API.DTOs;

public class OrderDTO
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public List<BookDTO> Books { get; set; }
}