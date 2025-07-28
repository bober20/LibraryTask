using System.Linq.Expressions;
using FluentResults;
using Library.Domain.Models;

namespace Library.Domain.Interfaces;

public interface IBookRepository
{
    Task<Result<Guid>> CreateBook(Book book, CancellationToken cancellationToken = default);
    Task<Result<Book?>> GetBook(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<Book>>> GetBooks(Expression<Func<Book, bool>>? predicate, CancellationToken cancellationToken = default);
}