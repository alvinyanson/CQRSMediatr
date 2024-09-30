using CQRSMediatr.API.Commands;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Responses;
using Mapster;
using MediatR;

namespace CQRSMediatr.API.Handlers
{
    public class GetDriverInfoHandler : IRequestHandler<CreateDriverInfoRequest, GetDriverResponse>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetDriverInfoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetDriverResponse> Handle(CreateDriverInfoRequest request, CancellationToken cancellationToken)
        {
            var driver = request.DriverRequest.Adapt<Driver>();

            await _unitOfWork.Drivers.Add(driver);

            await _unitOfWork.CompleteAsync();

            var result = driver.Adapt<GetDriverResponse>();

            return result;
        }
    }
}
