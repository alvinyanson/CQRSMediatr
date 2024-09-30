using CQRSMediatr.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CQRSMediatr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }
}
