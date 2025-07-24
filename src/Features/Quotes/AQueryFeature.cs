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
//         public Task<HandlerResult> Handle(Query request, CancellationToken ct)
//         {
//             throw new NotImplementedException();
//         }
//     }

//     public class AQueryEndpoint : IEndpoint
//     {
//         public void MapEndpoint(IEndpointRouteBuilder app)
//         {
//             throw new NotImplementedException();

            //    var handlerResponse = sender.Send();
            //    return handlerResponse switch
            //      {
            //         HappyResult h => Results.Ok(h.whatever),
            //         FailResult f => Results.BadRequest(f.ErrorMessage),
            //          _ => Results.StatusCode(500) 
            //      } ;
//         }
//     }
// }