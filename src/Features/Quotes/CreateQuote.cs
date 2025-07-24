using Api.Application.Abstractions;
using Api.Domain.Entities; 
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Application.Features.Quotes;

// Command and Result definitions
public sealed record CreateQuoteCommand(string Content, string Author) : IRequest<HandlerResult>;
public abstract record HandlerResult;
public sealed record HappyResult(int NewQuoteId) : HandlerResult;
public sealed record FailResult(string ErrorMessage) : HandlerResult;

// Handler
public sealed class CreateQuoteHandler(AppDbContext dbContext) : IRequestHandler<CreateQuoteCommand, HandlerResult>
{
    public async Task<HandlerResult> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content) || string.IsNullOrWhiteSpace(request.Author))
            return new FailResult("Author and Content are both required.");
        
        if (request.Content.Length < 6 || request.Author.Length < 6)
            return new FailResult("Author and Content must be at least six characters long.");
        
        var quote = new Quote
        {
            Content = request.Content,
            Author = request.Author
        };

        dbContext.Quotes.Add(quote);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new HappyResult(quote.Id);
    }
}

// Endpoint registration
public sealed class CreateQuoteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/quotes", async (ISender sender, CreateQuoteCommand command) =>
        {
            var result = await sender.Send(command);

            return result switch
            {
                HappyResult s => Results.CreatedAtRoute("GetQuoteById", new { id = s.NewQuoteId }),
                FailResult f => Results.BadRequest(f.ErrorMessage),
                _ => Results.StatusCode(500)
            };
        })
        .WithTags("Quotes")
        .WithName("CreateQuote");
    }
}