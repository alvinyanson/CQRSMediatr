using CQRSMediatr.Entities.DTOs.Requests;
using MediatR;

namespace CQRSMediatr.API.Commands
{
    public class UpdateDriverInfoRequest : IRequest<bool>
    {
        public UpdateDriverRequest DriverRequest { get; }

        public UpdateDriverInfoRequest(UpdateDriverRequest driverRequest)
        {
            DriverRequest = driverRequest;
        }
    }
}
