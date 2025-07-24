using Api.Application.Abstractions;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Quotes;

public static class GetQuoteById
{
    public record Query(int id) : IRequest<HandlerResult>;
    public abstract record HandlerResult;
    public sealed record HappyResult(string Content, string Author) : HandlerResult;
    public sealed record FailResult(string ErrorMessage) : HandlerResult;

    public class Handler(AppDbContext context) : IRequestHandler<Query, HandlerResult>
    {
        public async Task<HandlerResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var quote = await context.Quotes.FindAsync(request.id);
            if (quote is null)
                return new FailResult("Quote not found.");
            return new HappyResult(quote.Content, quote.Author);
        }
    }

    public class GetQuoteByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/mediatr/quotes/{id:int}", async (ISender sender, int id) =>
            {
                var response = await sender.Send(new GetQuoteById.Query(id));
                return response switch
                {
                    HappyResult s => Results.Ok(new HappyResult(s.Content, s.Author)),
                    FailResult f => Results.BadRequest(f.ErrorMessage),
                    _ => Results.StatusCode(500)
                };
            })
            .WithTags("mediatr")
            .WithName("GetQuoteByIdMediatR");
        }
    }
}