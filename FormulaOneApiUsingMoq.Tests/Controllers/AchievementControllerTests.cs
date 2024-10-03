
using AutoFixture;
using CQRSMediatr.API.Controllers;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Requests;
using CQRSMediatr.Entities.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace FormulaOneApiUsingMoq.Tests.Controllers
{
    public class AchievementControllerTests
    {

        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Fixture _fixture;
        private AchievementController _controller;

        public AchievementControllerTests()
        {
            _fixture = new Fixture();
            // fix circular dependency in navigation properties
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(behavior => _fixture.Behaviors.Remove(behavior));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Get_DriverAchievements_ReturnsNotFound()
        {
            //var driverAchievements = _fixture.Build<Achievement>()
            //            .Without(x => x.Driver)
            //            .Create();

            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Achievements.GetDriverAchievementAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Achievement?)null);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            var result = await _controller.GetDriverAchievements(driverId);

            var obj = (NotFoundObjectResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            obj.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Get_DriverAchievements_ReturnOk()
        {

            var driverAchievements = _fixture.Create<Achievement>();

            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Achievements
                    .GetDriverAchievementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(driverAchievements);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            var result = await _controller.GetDriverAchievements(driverId);

            var obj = (OkObjectResult) result;

            obj.StatusCode.Should().Be((int) HttpStatusCode.OK);
            obj.Value.Should().BeOfType<DriverAchievementResponse>();
        }


        [Fact]
        public async Task Post_DriverAchievements_ReturnsBadRequest()
        {
            var driverAchievements = _fixture.Create<CreateDriverAchievementRequest>();

            _unitOfWorkMock
                .Setup(x => x.Achievements
                    .Add(It.IsAny<Achievement>()))
                .ReturnsAsync(true);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            _controller.ModelState.AddModelError("WorldChampionship", "WorldChampionship is null"); // sample only

            var result = await _controller.AddDriverAchievement(driverAchievements);

            var obj = (BadRequestResult)result;

            obj.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_DriverAchievements_ReturnsOk()
        {
            var driverAchievements = _fixture.Create<CreateDriverAchievementRequest>();

            _unitOfWorkMock
                .Setup(x => x.Achievements
                    .Add(It.IsAny<Achievement>()))
                .ReturnsAsync(true);
            
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            var result = await _controller.AddDriverAchievement(driverAchievements);

            var obj = (CreatedAtActionResult) result;

            obj.StatusCode.Should().Be((int) HttpStatusCode.Created);
            obj.Value.Should().BeOfType<Achievement>();
        }

        [Fact]
        public async Task Post_DriverAchievements_ReturnsAchievement()
        {
            var driverAchievements = _fixture.Create<CreateDriverAchievementRequest>();

            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Achievements
                    .Add(It.IsAny<Achievement>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            var result = await _controller.AddDriverAchievement(driverAchievements);

            var obj = (CreatedAtActionResult) result;

            result.Should().BeOfType<CreatedAtActionResult>()
                                .Which.RouteValues.Should().ContainKey("driverId");

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.Value.Should().Be(obj?.Value);
        }

        [Fact]
        public async Task Put_DriverAchievements_ReturnsBadRequest()
        {
            var driverAchievements = _fixture.Create<UpdateDriverAchievementRequest>();

            _unitOfWorkMock
                .Setup(x => x.Achievements
                    .Add(It.IsAny<Achievement>()))
                .ReturnsAsync(true);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            _controller.ModelState.AddModelError("WorldChampionship", "WorldChampionship is null"); // sample only

            var result = await _controller.UpdateDriverAchievement(driverAchievements);

            var obj = (BadRequestResult)result;

            obj.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_DriverAchievements_ReturnsNoContent()
        {
            var driverAchievements = _fixture.Create<UpdateDriverAchievementRequest>();

            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Achievements
                    .Update(It.IsAny<Achievement>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new AchievementController(_unitOfWorkMock.Object);

            var result = await _controller.UpdateDriverAchievement(driverAchievements);

            var obj = (NoContentResult) result;

            obj.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        }

    }
}
