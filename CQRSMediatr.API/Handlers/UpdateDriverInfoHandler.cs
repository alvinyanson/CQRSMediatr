using CQRSMediatr.API.Commands;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using Mapster;
using MediatR;

namespace CQRSMediatr.API.Handlers
{
    public class UpdateDriverInfoHandler : IRequestHandler<UpdateDriverInfoRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDriverInfoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> Handle(UpdateDriverInfoRequest request, CancellationToken cancellationToken)
        {
            var result = request.DriverRequest.Adapt<Driver>();

            await _unitOfWork.Drivers.Update(result);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
