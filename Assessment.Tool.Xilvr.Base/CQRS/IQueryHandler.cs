using MediatR;

namespace Assessment.Tool.Xilvr.Base.CQRS;

//
// Summary:
//     Query Handler interface
//
// Type parameters:
//   TQuery:
//
//   TResponse:
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
}
