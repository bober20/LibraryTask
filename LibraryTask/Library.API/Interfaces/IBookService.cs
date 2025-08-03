using System.Linq.Expressions;
using FluentResults;
using Library.API.DTOs;
using Library.Domain.Models;

namespace Library.API.Interfaces;

public interface IBookService
{
    Task<Result<Guid>> CreateBook(BookDTO book, CancellationToken cancellationToken = default);

    Task<Result<BookDTO>> GetBook(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<BookDTO>>> GetBooks(Expression<Func<Book, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}