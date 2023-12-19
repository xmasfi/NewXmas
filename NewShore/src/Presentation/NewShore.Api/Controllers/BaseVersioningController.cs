using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NewShore.Api.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class BaseVersioningController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
