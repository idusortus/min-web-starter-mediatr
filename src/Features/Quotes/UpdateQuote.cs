using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Quotes;

public static class UpdateQuote
{
    public record Query(int Id, string? Author, string? Content) : IRequest<HandlerResult>;

    public abstract record HandlerResult;
    public sealed record HappyResult(Quote quote) : HandlerResult;
    public sealed record FailResult(string ErrorMessage) : HandlerResult;

    public class Handler(AppDbContext context) : IRequestHandler<Query, HandlerResult>
    {
        public async Task<HandlerResult> Handle(Query request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Content) || string.IsNullOrWhiteSpace(request.Author))
                return new FailResult("Author and Content must be populated");
            if (request.Content.Length < 6 || request.Author.Length < 6)
                return new FailResult("Author and Content must have at least six characters.");

            var existingQuote = await context.Quotes.FindAsync(request.Id, ct);
            if (existingQuote is null)
                return new FailResult($"Quote with id:{request.Id} not found.");

            existingQuote.Author = request.Author;
            existingQuote.Content = request.Content;
            await context.SaveChangesAsync(ct);

            return new HappyResult(existingQuote);
        }
    }

    public class UpdateQuoteEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("mediatr/quotes", async (ISender sender, Query q, CancellationToken ct) =>
            {
                var handlerResponse = await sender.Send(q, ct);

                return handlerResponse switch
                {
                    HappyResult h => Results.Ok(h.quote),
                    FailResult f => Results.BadRequest(f.ErrorMessage),
                    _ => Results.StatusCode(500)
                };
            })
            .WithTags("mediatr");
        }
    }
}