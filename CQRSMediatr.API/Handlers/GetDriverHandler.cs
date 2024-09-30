using CQRSMediatr.API.Queries;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Responses;
using Mapster;
using MediatR;

namespace CQRSMediatr.API.Handlers
{
    public class GetDriverHandler : IRequestHandler<GetDriverQuery, GetDriverResponse>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetDriverHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetDriverResponse> Handle(GetDriverQuery request, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.Drivers.GetById(request.DriverId);

            if (driver == null)
                return null;

            var result = driver.Adapt<GetDriverResponse>();

            return result;
        }
    }
}
