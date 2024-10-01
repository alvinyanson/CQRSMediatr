using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.Entities.DbSet;
using CQRSMediatr.Entities.DTOs.Requests;
using CQRSMediatr.Entities.DTOs.Responses;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CQRSMediatr.API.Controllers
{
    public class DriversController : BaseController
    {
        public DriversController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {


        }

        [HttpGet]
        [Route("{driverId:Guid}")]
        public async Task<IActionResult> GetDriver(Guid driverId)
        {
            var driver = await _unitOfWork.Drivers.GetById(driverId);

            if (driver == null)
                return NotFound();

            var result = driver.Adapt<GetDriverResponse>();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _unitOfWork.Drivers.All();

            var result = drivers.Adapt<IEnumerable<GetDriverResponse>>().ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] CreateDriverRequest driver)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var result = driver.Adapt<Driver>();

            await _unitOfWork.Drivers.Add(result);

            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetDriver), new { driverId = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest driver)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = driver.Adapt<Driver>();

            await _unitOfWork.Drivers.Update(result);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{driverId:Guid}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            var driver = await _unitOfWork.Drivers.GetById(driverId);

            if (driver == null)
                return NotFound();

            await _unitOfWork.Drivers.Delete(driverId);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

    } 
}
