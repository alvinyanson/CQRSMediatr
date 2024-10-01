
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
    public class DriverControllerTests
    {
        private DriversController _driversController;
        private IUnitOfWork _unitOfWork;


        public DriverControllerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();


            // SUT
            _driversController = new DriversController(_unitOfWork);
        }


        [Fact]
        public async Task DriversController_GetDriver_ReturnsSuccess()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            var driver = A.Fake<Driver>();
            A.CallTo(() => _unitOfWork.Drivers.GetById(driverId)).Returns(driver);


            // Act
            var result = await _driversController.GetDriver(driverId);


            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<GetDriverResponse>();
        }

        [Fact]
        public async Task DriversController_GetDriver_ReturnNotFound_WhenDriverDoesNotExist()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            A.CallTo(() => _unitOfWork.Drivers.GetById(driverId)).Returns(Task.FromResult<Driver>(null));


            // Act
            var result = await _driversController.GetDriver(driverId);


            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DriversController_GetAllDrivers_ReturnsSuccess()
        {
            // Arrange
            var drivers = A.Fake<IEnumerable<Driver>>();
            A.CallTo(() => _unitOfWork.Drivers.All()).Returns(drivers);


            // Act
            var result = await _driversController.GetAllDrivers();


            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<List<GetDriverResponse>>();
        }

        [Fact]
        public async Task DriversController_AddDriver_ReturnsSuccess()
        {
            // Arrange
            var driver = A.Fake<Driver>();
            var driverRequest = driver.Adapt<CreateDriverRequest>();
            
            A.CallTo(() => _unitOfWork.Drivers.Add(driver)).Returns(Task.FromResult(true));
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(true));


            // Act
            var result = await _driversController.AddDriver(driverRequest);


            // Assert
            var createdResult = result as CreatedAtActionResult;

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.RouteValues.Should().ContainKey("driverId");

            result.Should().BeOfType<CreatedAtActionResult>()
                .Which.Value.Should().Be(createdResult?.Value);
        }

        [Fact]
        public async Task DriversController_AddDriver_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var driverRequest = A.Fake<CreateDriverRequest>();

            _driversController.ModelState.AddModelError("test", "test");

            // Act
            var result = await _driversController.AddDriver(driverRequest);


            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DriversController_UpdateDriver_ReturnsSuccess()
        {
            // Arrange
            var driver = A.Fake<Driver>();
            var driverRequest = driver.Adapt<UpdateDriverRequest>();

            A.CallTo(() => _unitOfWork.Drivers.Update(driver)).Returns(Task.FromResult(true));
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(true));


            // Act
            var result = await _driversController.UpdateDriver(driverRequest);


            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DriversController_UpdateDriver_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var driverRequest = A.Fake<UpdateDriverRequest>();

            _driversController.ModelState.AddModelError("test", "test");

            // Act
            var result = await _driversController.UpdateDriver(driverRequest);


            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DriversController_DeleteDriver_ReturnsSuccess()
        {
            // Arrange
            var driverId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.Drivers.Delete(driverId)).Returns(Task.FromResult(true));
            A.CallTo(() => _unitOfWork.CompleteAsync()).Returns(Task.FromResult(true));


            // Act
            var result = await _driversController.DeleteDriver(driverId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task DriversController_DeleteDriver_ReturnsNotFound_WhenDriverDoesNotExist()
        {
            // Arrange
            var driverId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.Drivers.GetById(driverId)).Returns(Task.FromResult<Driver>(null));


            // Act
            var result = await _driversController.DeleteDriver(driverId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
