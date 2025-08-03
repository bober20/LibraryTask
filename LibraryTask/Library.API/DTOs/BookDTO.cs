namespace Library.API.DTOs;

public class BookDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
}