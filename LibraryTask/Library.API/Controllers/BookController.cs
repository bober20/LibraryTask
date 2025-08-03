using Library.API.DTOs;
using Library.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

public class BookController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    [Route("api/[controller]/{id:guid}")]
    public async Task<IActionResult> GetBook(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await bookService.GetBook(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return NotFound(result.Errors?.FirstOrDefault()?.Message ?? "Book with this id is not found.");
    }

    [HttpGet]
    [Route("api/[controller]")]
    public async Task<IActionResult> GetBooks([FromQuery] string? title, [FromQuery] DateOnly? publishDate, CancellationToken cancellationToken = default)
    {
        var result = await bookService.GetBooks(book => 
            (string.IsNullOrEmpty(title) || book.Title.ToLower().Contains(title.ToLower())) &&
            (!publishDate.HasValue || book.PublishDate == publishDate), cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return NotFound(result.Errors?.FirstOrDefault()?.Message ?? "There are no books.");
    }
    
    [HttpPost]
    [Route("api/[controller]")]
    public async Task<IActionResult> CreateBook([FromBody] BookDTO book, CancellationToken cancellationToken = default)
    {
        if (book == null)
        {
            return BadRequest("Book data is required.");
        }

        var result = await bookService.CreateBook(book, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetBook), new { id = result.Value }, result.Value);
        }
        
        return BadRequest(result.Errors?.FirstOrDefault()?.Message ?? "Failed to create book.");
    }
}