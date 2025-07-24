using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;//[FromQuery]

namespace Api.Features.Quotes;

public static class GetQuotes
{
    public record Query(int pageNumber, int pageSize) : IRequest<HandlerResult>;
    public abstract record HandlerResult;
    public sealed record HappyResult(PaginatedResult<Quote> quotes) : HandlerResult;
    public sealed record FailResult(string ErrorMessage) : HandlerResult;

    public class Handler(AppDbContext context) : IRequestHandler<Query, HandlerResult>
    {
        public async Task<HandlerResult> Handle(Query request, CancellationToken ct)
        {
            var pagination = new PaginationParams
            {
                PageNumber = request.pageNumber,
                PageSize = request.pageSize
            };

            var query = context.Quotes.OrderBy(q => q.Id);
            var result = await query.ToPaginatedResultAsync(pagination, ct);
            return new HappyResult(result);
        }
    }

    public class GetQuotesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("mediatr/quotes", async (
                ISender sender,
                CancellationToken ct,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10) =>
            {
                var handlerResponse = await sender.Send(new Query(pageNumber, pageSize), ct);

                return handlerResponse switch
                {
                    HappyResult h =>Results.Ok(handlerResponse),
                    FailResult f =>Results.BadRequest(f.ErrorMessage),
                    _ => Results.StatusCode(500)
                };
            })
            .WithTags("mediatr");
        }
    }
}