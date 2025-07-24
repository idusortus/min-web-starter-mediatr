using Api.Abstractions;
using Api.Infrastructure.Persistence;
using MediatR;

public static class GetQuoteById
{
    public record Query(int id) : IRequest<Response>;
    public record Response(int QuoteId, string Content, string Author);

    public class Handler(AppDbContext context) : IRequestHandler<Query, Response?>
    {
        public async Task<Response?> Handle(Query request, CancellationToken cancellationToken)
        {
            var quote = await context.Quotes.FindAsync(request.id);
            if (quote is null) return null;
            return new Response(quote.Id, quote.Content, quote.Author);
        }
    }

    public class GetQuoteByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/mediatr/quotes/{id:int}", async (ISender sender, int id) =>
            {
                var response = await sender.Send(new GetQuoteById.Query(id));
                return (response == null)
                    ? Results.NotFound()
                    : Results.Ok(response);
            })
            .WithTags("mediatr")
            .WithName("GetQuoteByIdMediatR");
        }
    }
}