using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiNew.Models;
using apiNew.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;

namespace apiNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DriversController : ControllerBase
    {
       private readonly IDriverRepository _driverRepository;

        public DriversController(IDriverRepository driverRepository)
        {
           _driverRepository = driverRepository;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drivers>>> GetDrivers()
        {
           return Ok(await _driverRepository.GetAllAsync());
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drivers>> GetDrivers(int id)
        {
           var driver = await _driverRepository.GetByIdAsync(id);

            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        // PUT: api/Drivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrivers(int id, Drivers drivers)
        {
            if (id != drivers.Id)
            {
                return BadRequest();
            }

            await _driverRepository.UpdateAsync(drivers);
            return NoContent();
        }

        // POST: api/Drivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Drivers>> PostDrivers(Drivers drivers)
        {
            await _driverRepository.AddAsync(drivers);
            return CreatedAtAction("GetDrivers", new { id = drivers.Id }, drivers);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrivers(int id)
        {
            var drivers = await _driverRepository.GetByIdAsync(id);
            if (drivers == null)
            {
                return NotFound();
            }

            await _driverRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
