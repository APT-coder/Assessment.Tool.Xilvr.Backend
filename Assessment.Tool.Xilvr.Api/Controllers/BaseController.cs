using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Tool.Xilvr.Api.Controllers;

[Route("xilvr/api/v1")]
[ApiController]
[ProducesResponseType(typeof(ProblemDetails), 500)]
public class BaseController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
