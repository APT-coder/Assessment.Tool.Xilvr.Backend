using MediatR;

namespace Assessment.Tool.Xilvr.Base.CQRS;

//
// Summary:
//     Query interfcae
//
// Type parameters:
//   TResponse:
public interface IQuery<out TResponse> : IRequest<TResponse>, IBaseRequest
{
}
