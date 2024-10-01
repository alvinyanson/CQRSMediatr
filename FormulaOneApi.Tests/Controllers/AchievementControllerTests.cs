
using CQRSMediatr.API.Controllers;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Requests;
using CQRSMediatr.Entities.DTOs.Responses;
using FakeItEasy;
using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOneApi.Tests.Controllers
{
    public class AchievementControllerTests
    {
        private AchievementController _achievementController;
        private IUnitOfWork _unitOfWork;


        public AchievementControllerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();


            // SUT
            _achievementController = new AchievementController(_unitOfWork);
        }


        [Fact]
        public async Task AchievementController_GetDriverAchievements_ReturnsSuccess()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            var achievement = A.Fake<Achievement>();
            A.CallTo(() => _unitOfWork.Achievements.GetDriverAchievementAsync(driverId)).Returns(Task.FromResult<Achievement>(achievement));

            // Act
            var result = await _achievementController.GetDriverAchievements(driverId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<DriverAchievementResponse>();
        }

        [Fact]
        public async Task AchievementController_GetDriverAchievements_ReturnNotFound_WhenDriverAchievementIsNull()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            A.CallTo(() => _unitOfWork.Achievements.GetDriverAchievementAsync(driverId)).Returns(Task.FromResult<Achievement>(null));

            // Act
            var result = await _achievementController.GetDriverAchievements(driverId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AchievementController_AddDriverAchievement_ReturnsSuccess()
        {
            // Arrange
            var achievementRequest = A.Fake<CreateDriverAchievementRequest>();

            var achievement = achievementRequest.Adapt<Achievement>();

            A.CallTo(() => _unitOfWork.Achievements.Add(achievement)).Returns(Task.FromResult(true));
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(true));

            // Act
            var result = await _achievementController.AddDriverAchievement(achievementRequest);

            // Assert
            var createdResult = result as CreatedAtActionResult;

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.RouteValues.Should().ContainKey("driverId");

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.Value.Should().Be(createdResult?.Value);
        }

        [Fact]
        public async Task AchievementController_AddDriverAchievement_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var achievementRequest = A.Fake<CreateDriverAchievementRequest>();
            _achievementController.ModelState.AddModelError("WorldChampionship", "WorldChampionship is null"); // sample only


            // Act
            var result = await _achievementController.AddDriverAchievement(achievementRequest);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AchievementController_UpdateDriverAchievement_ReturnsSuccess()
        {
            // Arrange
            var achievementRequest = A.Fake<UpdateDriverAchievementRequest>();

            var achievement = achievementRequest.Adapt<Achievement>();

            A.CallTo(() => _unitOfWork.Achievements.Update(achievement)).Returns(Task.FromResult(true));
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(true));

            // Act
            var result = await _achievementController.UpdateDriverAchievement(achievementRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AchievementController_UpdateDriverAchievement_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var achievementRequest = A.Fake<UpdateDriverAchievementRequest>();
            _achievementController.ModelState.AddModelError("WorldChampionship", "WorldChampionship is null"); // sample only


            // Act
            var result = await _achievementController.UpdateDriverAchievement(achievementRequest);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

    }
}
