using Api.Application.Abstractions;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Quotes;

public static class DeleteQuote
{
    // The Command now declares it will return our business outcome enum.
    public record Command(int Id) : IRequest<DeleteStatus>;
    // The Handler deals only with business logic and returns
    // a simple enum to describe the result. 
    public class Handler(AppDbContext context) : IRequestHandler<Command, DeleteStatus>
    {
        public async Task<DeleteStatus> Handle(Command request, CancellationToken ct)
        {
            var quote = await context.Quotes.FindAsync(new object[] { request.Id }, ct);
            if (quote is null)            
                return DeleteStatus.Failure;            

            context.Quotes.Remove(quote);
            await context.SaveChangesAsync(ct);

            return DeleteStatus.Success;
        }
    }

    public class DeleteQuoteEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("mediatr/quotes/{id:int}", async (ISender sender, int id, CancellationToken ct) =>
            {
                // Create the command to send
                var command = new Command(id);

                // Send the command and get the business result back
                var result = await sender.Send(command, ct);

                // The Endpoint's job: Translate the business result into an HTTP result.
                return result switch
                {
                    DeleteStatus.Success => Results.NoContent(),
                    DeleteStatus.Failure => Results.NotFound(),
                    _ => Results.Problem("An unexpected error occurred.")
                };
            })
            .WithTags("mediatr")
            .WithName("DeleteQuote");
        }
    }
}