using Api.Application.Abstractions;
using Api.Infrastructure.Persistence;

namespace Api.Endpoints;

class GetQuoteById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // // Optional, useful for replicating Controller grouping. Not as much utility in VSA/Modular Monolith
        // var quotesGroup = app.MapGroup("/api/v1/quotes")
        //     .WithTags("Quotes");

        app.MapGet("quotes/{id:int}", async (AppDbContext context, int id) =>
        {
            var resp = await context.Quotes.FindAsync(id);
            return (resp is null)
                ? Results.NotFound()
                : Results.Ok(resp);
        })
        .WithTags("Quotes")
        .WithName("GetQuoteById");
    }
}
