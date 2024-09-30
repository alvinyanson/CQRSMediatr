using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Requests;
using CQRSMediatr.Entities.DTOs.Responses;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace CQRSMediatr.API.Profiles
{
    public class MapsterProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // request to domain
            config.NewConfig<CreateDriverAchievementRequest, Achievement>()
                .Map(dest => dest.RaceWins, src => src.Wins)
                .Map(dest => dest.Status, src => 1)
                .Map(dest => dest.AddedDate, src => DateTime.UtcNow)
                .Map(dest => dest.UpdatedDate, src => DateTime.UtcNow);


            config.NewConfig<UpdateDriverAchievementRequest, Achievement>()
                .Map(dest => dest.RaceWins, src => src.Wins)
                .Map(dest => dest.UpdatedDate, src => DateTime.UtcNow);


            config.NewConfig<CreateDriverRequest, Driver>()
                .Map(dest => dest.Status, src => 1)
                .Map(dest => dest.AddedDate, src => DateTime.UtcNow)
                .Map(dest => dest.UpdatedDate, src => DateTime.UtcNow);


            config.NewConfig<UpdateDriverRequest, Driver>()
                .Map(dest => dest.UpdatedDate, src => DateTime.UtcNow);



            // domain to response
            // source to destination
            config.NewConfig<Achievement, CreateDriverAchievementRequest>()
                .Map(dest => dest.Wins, src => src.RaceWins);


            config.NewConfig<Achievement, DriverAchievementResponse>()
                 .Map(dest => dest.Wins, src => src.RaceWins);


            config.NewConfig<Driver, GetDriverResponse>()
                .Map(dest => dest.DriverId, src => src.Id)
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
                
        }
    }
}
