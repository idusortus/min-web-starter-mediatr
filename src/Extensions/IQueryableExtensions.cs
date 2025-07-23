using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        PaginationParams pagination)
    {
        return query
            .Skip(pagination.Skip)
            .Take(pagination.PageSize);
    }

    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
    this IQueryable<T> query,
    PaginationParams pagination,
    CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, totalCount, pagination.PageNumber, pagination.PageSize);
    }
}

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
    public PaginatedResult() { }

    public PaginatedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class PaginationParams
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int Skip => (PageNumber - 1) * PageSize;
}