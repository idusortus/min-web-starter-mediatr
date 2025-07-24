// using Api.Application.Abstractions;
// using MediatR;

// public static class AQueryFeature
// {

//     public record Query : IRequest<HandlerResult>;
//     public abstract record HandlerResult;
//     public sealed record HappyResult : HandlerResult;
//     public sealed record FailResult : HandlerResult;

//     public class Handler : IRequestHandler<Query, HandlerResult>
//     {
//         public Task<HandlerResult> Handle(Query request, CancellationToken cancellationToken)
//         {
//             throw new NotImplementedException();
//         }
//     }

//     public class AQueryEndpoint : IEndpoint
//     {
//         public void MapEndpoint(IEndpointRouteBuilder app)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }