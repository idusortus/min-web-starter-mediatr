using Api.Abstractions;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Quotes;

public static class NewCommandFeature
{
    public record Command(string data) : IRequest<int>;

    public class Handler(AppDbContext context) : IRequestHandler<Command, int>
    {
        public Task<int> Handle(Command request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }

    public class NewCommandFeatureEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("url", async (ISender sender, Command Command) =>
            {
                int result = 1;
                return Results.Ok(result);
            });
        }
    }
}