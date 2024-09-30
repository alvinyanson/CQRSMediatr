using CQRSMediatr.Entities.DTOs.Responses;
using MediatR;

namespace CQRSMediatr.API.Queries
{
    public class GetDriverQuery : IRequest<GetDriverResponse>
    {
        public Guid DriverId { get; }

        public GetDriverQuery(Guid driverId)
        {
            DriverId = driverId;
        }
    }
}
