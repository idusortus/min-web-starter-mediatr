using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public class DeleteQuote : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("quotes/{id:int}", async (AppDbContext context, int id, CancellationToken ct) =>
        {
            Quote? quote = await context.Quotes.FindAsync(id, ct);

            if (quote is null)
                return Results.NotFound();

            context.Quotes.Remove(quote);
            await context.SaveChangesAsync(ct);
            return Results.NoContent();
        })
        .WithTags("Quotes");
    }
}
