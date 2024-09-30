using CQRSMediatr.DataService.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSMediatr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMediator _mediator;

        public BaseController(
            IUnitOfWork unitOfWork,
            IMediator mediator
            )
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }


    }
}
