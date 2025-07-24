using Api.Application.Abstractions;
using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Api.Endpoints;

public class UpdateQuote : IEndpoint
{
    public record QuoteDto(int Id, string? Content, string? Author);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("quotes", async ([FromServices] AppDbContext context, [FromBody] QuoteDto qdto, CancellationToken ct) =>
        {
            if (qdto.Id < 1)
                return Results.BadRequest("Valid Id required.");

            if (qdto.Author is null || qdto.Content is null)
                return Results.BadRequest("Content or Author required.");
             if (qdto.Content.Length < 6 || qdto.Author.Length < 6)
                return Results.BadRequest("Author and Content must have at least six characters.");
                
            var existingQuote = await context.Quotes.FindAsync(qdto.Id, ct);

            if (existingQuote is null)
                return Results.NotFound();
            if (qdto.Author is not null && qdto.Author.Length > 6)
                existingQuote.Author = qdto.Author;
            if (qdto.Content is not null && qdto.Content.Length > 6)
                existingQuote.Content = qdto.Content;

            await context.SaveChangesAsync(ct);
            return Results.Ok(existingQuote);

        })
        .WithTags("Quotes");
    }
}