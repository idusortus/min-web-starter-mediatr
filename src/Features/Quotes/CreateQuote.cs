using Api.Abstractions;
using Api.Domain.Entities; // Assuming your Quote entity is in this namespace
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class CreateQuote
{
    public record Command(string Content, string Author) : IRequest<int>;

    public class Handler(AppDbContext context) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var quote = new Quote
            {
                Content = request.Content,
                Author = request.Author
            };

            context.Quotes.Add(quote);

            await context.SaveChangesAsync(cancellationToken);
            return quote.Id;
        }
    }

    public class CreateQuoteEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/mediatr/quotes", async (ISender sender,  Command command) =>
            {
                if (command.Content is null || command.Author is null)
                    return Results.BadRequest("Author and Content are required.");
                if (command.Content.Length < 6 || command.Author.Length < 6)
                    return Results.BadRequest("Author and Content must have at least six characters.");
                var newQuoteId = await sender.Send(command);
                
                return Results.CreatedAtRoute("GetQuoteByIdMediator", new { id = newQuoteId });
            })
            .WithTags("mediatr")
            .WithName("CreateQuoteMediatR");
        }
    }
}