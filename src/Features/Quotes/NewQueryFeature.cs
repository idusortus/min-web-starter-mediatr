using Api.Abstractions;
using Api.Infrastructure.Persistence;
using MediatR;

public static class NewQueryFeature
{
    public record Query : IRequest;
    public record Response(int Id, string Foo, string Bar);

    public class Handler(AppDbContext context) : IRequestHandler<Query>
    {
        public Task Handle(Query request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class NewFeatureEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/endpointname", async (ISender sender) =>
            {
                //dostuff
                var magicInt = new Random().Next(1, 3);
                return (magicInt / 2 == 1)
                    ? Results.NotFound()
                    : Results.Ok();
            })
            .WithTags("endpoints")
            .WithName("GreatName");
        }
    }
}