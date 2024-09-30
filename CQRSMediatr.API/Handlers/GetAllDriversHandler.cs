using CQRSMediatr.API.Queries;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DTOs.Responses;
using Mapster;
using MediatR;

namespace CQRSMediatr.API.Handlers
{
    public class GetAllDriversHandler : IRequestHandler<GetAllDriversQuery, IEnumerable<GetDriverResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetAllDriversHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetDriverResponse>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _unitOfWork.Drivers.All();

            var result = drivers.Adapt<IEnumerable<GetDriverResponse>>();

            return result;
        }
    }
}
