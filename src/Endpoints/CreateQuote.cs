using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using Microsoft.VisualBasic;

namespace Api.Endpoints;

public class CreateQuote : IEndpoint
{
    public record QuoteDto(string Content, string Author);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("quotes", async (AppDbContext context, QuoteDto quote, CancellationToken ct) =>
        {
            if (quote.Content is null || quote.Author is null)
                return Results.BadRequest("Author and Content are required.");
            if (quote.Content.Length < 6 || quote.Author.Length < 6)
                return Results.BadRequest("Author and Content must have at least six characters.");
            var newQuote = new Quote { Content = quote.Content, Author = quote.Author };
            var result = await context.Quotes.AddAsync(newQuote, ct);
            await context.SaveChangesAsync(ct);
            return Results.CreatedAtRoute("GetQuoteById", new { id = newQuote.Id }, newQuote.Id);
        })
        .WithTags("Quotes");
    }
}