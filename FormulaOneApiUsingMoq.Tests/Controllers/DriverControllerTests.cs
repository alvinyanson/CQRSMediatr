
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
    public class DriverControllerTests
    {

        private Mock<IUnitOfWork> _unitOfWorkMock;
        private DriversController _controller;
        private Fixture _fixture;

        public DriverControllerTests()
        {
            _fixture = new Fixture();
            // fix circular dependency in navigation properties
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(behavior => _fixture.Behaviors.Remove(behavior));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }


        [Fact]
        public async Task Get_Driver_ReturnsNotFound()
        {
            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .GetById(It.IsAny<Guid>()))
                .ReturnsAsync((Driver?)null);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.GetDriver(driverId);

            var obj = (NotFoundResult) result;

            obj.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
            obj.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_Driver_ReturnsOk()
        {
            var driver = _fixture.Create<Driver>();
            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .GetById(It.IsAny<Guid>()))
                .ReturnsAsync(driver);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.GetDriver(driverId);

            var obj = (OkObjectResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.OK);
            obj.Value.Should().BeOfType<GetDriverResponse>();
        }

        [Fact]
        public async Task Get_Drivers_ReturnsOk()
        {
            var drivers = _fixture.CreateMany<Driver>(10).ToList();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .All())
                .ReturnsAsync(drivers);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.GetAllDrivers();

            var obj = (OkObjectResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.OK);
            obj.Value.Should().BeOfType<List<GetDriverResponse>>();
        }

        [Fact]
        public async Task Post_Driver_ReturnsBadRequest()
        {
            var driver = _fixture.Create<CreateDriverRequest>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .Add(It.IsAny<Driver>()))
                .ReturnsAsync(true);

            _controller = new DriversController(_unitOfWorkMock.Object);
            _controller.ModelState.AddModelError("DriverNumber", "DriverNumber is null"); // sample only

            var result = await _controller.AddDriver(driver);

            var obj = (BadRequestResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_Driver_ReturnsCreatedAt()
        {
            var driver = _fixture.Create<CreateDriverRequest>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .Add(It.IsAny<Driver>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.AddDriver(driver);

            var obj = (CreatedAtActionResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.Created);
            obj.Value.Should().BeOfType<Driver>();
        }

        [Fact]
        public async Task Post_Driver_ReturnsDriver()
        {
            var driver = _fixture.Create<CreateDriverRequest>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .Add(It.IsAny<Driver>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.AddDriver(driver);

            var obj = (CreatedAtActionResult)result;

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.RouteValues.Should().ContainKey("driverId");

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.Value.Should().Be(obj?.Value);
        }

        [Fact]
        public async Task Put_Driver_ReturnsBadRequest()
        {
            var driver = _fixture.Create<UpdateDriverRequest>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .Add(It.IsAny<Driver>()))
                .ReturnsAsync(true);

            _controller = new DriversController(_unitOfWorkMock.Object);
            _controller.ModelState.AddModelError("DriverNumber", "DriverNumber is null"); // sample only

            var result = await _controller.UpdateDriver(driver);

            var obj = (BadRequestResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_Driver_ReturnsNoContent()
        {
            var driver = _fixture.Create<UpdateDriverRequest>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .Add(It.IsAny<Driver>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.UpdateDriver(driver);

            var obj = (NoContentResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Driver_ReturnsNotFound()
        {
            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .GetById(It.IsAny<Guid>()))
                .ReturnsAsync((Driver?) null);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.DeleteDriver(driverId);

            var obj = (NotFoundResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Driver_ReturnsNoContent()
        {
            var driver = _fixture.Create<Driver>();
            var driverId = _fixture.Create<Guid>();

            _unitOfWorkMock
                .Setup(x => x.Drivers
                    .GetById(It.IsAny<Guid>()))
                .ReturnsAsync(driver);

            _controller = new DriversController(_unitOfWorkMock.Object);

            var result = await _controller.DeleteDriver(driverId);

            var obj = (NoContentResult)result;

            obj.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

    }
}
