using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Library.API.DTOs;
using Library.API.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models;

namespace Library.API.Services;

public class BookService(IBookRepository bookRepository, IMapper mapper) : IBookService
{
    public async Task<Result<Guid>> CreateBook(BookDTO book, CancellationToken cancellationToken = default)
    {
        var bookModel = mapper.Map<Book>(book);
        return await bookRepository.CreateBook(bookModel, cancellationToken);
    }

    public async Task<Result<BookDTO>> GetBook(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await bookRepository.GetBook(id, cancellationToken);
        if (result.IsFailed)
        {
            return Result.Fail("There is no book with this id");
        }
        
        return Result.Ok(mapper.Map<BookDTO>(result.Value));
    }
    
    public async Task<Result<List<BookDTO>>> GetBooks(Expression<Func<Book, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var result = await bookRepository.GetBooks(predicate, cancellationToken);
        if (result.IsFailed)
        {
            return Result.Fail("There are no books that will match the specified query");
        }
        
        return Result.Ok(result.Value.Select(book => mapper.Map<BookDTO>(book)).ToList());
    }
}