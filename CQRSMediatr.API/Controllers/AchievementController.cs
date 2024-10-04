using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Requests;
using CQRSMediatr.Entities.DTOs.Responses;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CQRSMediatr.API.Controllers
{
    public class AchievementController : BaseController
    {

        public AchievementController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet("TestConnection")]
        public IActionResult TestConnection() {
            return Ok("Connection is OK");
        }


        [HttpGet("{driverId:Guid}")]
        public async Task<IActionResult> GetDriverAchievements(Guid driverId)
        {
            var driverAchievements = await _unitOfWork.Achievements.GetDriverAchievementAsync(driverId);

            if(driverAchievements == null)
                return NotFound("Achievements not found.");

            var result = driverAchievements.Adapt<DriverAchievementResponse>();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddDriverAchievement([FromBody] CreateDriverAchievementRequest driverAchievement)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = driverAchievement.Adapt<Achievement>();

            await _unitOfWork.Achievements.Add(result);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDriverAchievement([FromBody] UpdateDriverAchievementRequest driverAchievement)
        {
            if (!ModelState.IsValid) 
                return BadRequest();

            var result = driverAchievement.Adapt<Achievement>();

            await _unitOfWork.Achievements.Update(result);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
