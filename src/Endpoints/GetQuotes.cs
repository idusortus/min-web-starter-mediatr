using Api.Application.Abstractions;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints;

public class GetQuotes : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("quotes", async (
            [FromServices] AppDbContext context,
            CancellationToken ct,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10) =>
        {
            var pagination = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var query = context.Quotes.OrderBy(q => q.Id);

            var result = await query.ToPaginatedResultAsync(pagination, ct);

            return Results.Ok(result);
        })
        .WithTags("Quotes");
    }
}
