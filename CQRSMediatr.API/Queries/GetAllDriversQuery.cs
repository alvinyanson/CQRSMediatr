using CQRSMediatr.Entities.DTOs.Responses;
using MediatR;

namespace CQRSMediatr.API.Queries
{
    public class GetAllDriversQuery : IRequest<IEnumerable<GetDriverResponse>>
    {

    }
}
