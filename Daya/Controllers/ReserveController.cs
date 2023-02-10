using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveService.Interface;
using ReserveService.Model;
using ReserveService.Model.Dto;

namespace Daya.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveRepo rp;

        public ReserveController(IReserveRepo rp)
        {
            this.rp = rp;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var all = rp.GetAll();

            if (all is null)
                return StatusCode(404, new StandardResult() { Success = true, Message = "there is no Reserve in database" });

            return StatusCode(200, new StandardResult<List<Reserve>>() { Success = true, Message = "All Reserves returned .", Result = all });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reserve = rp.GetById(id);
            if (reserve is null)
                return StatusCode(404, new StandardResult() { Success = false, Message = " Reserve not found ." });

            return StatusCode(200, new StandardResult<Reserve>() { Success = true, Message = " Reserve founded .", Result = reserve });
        }

        [HttpPost, Authorize(Policy = "JustActive")]
        public IActionResult Create(AddReserveDto reserveDto)
        {
            if (rp.ExistForAdd(reserveDto.ReserveDate, reserveDto.LocationId))
                return StatusCode(500, new StandardResult { Success = false, Message = "you can't reserve this location on this date" });

            var reserverId = Guid.Parse(HttpContext.User.Identity.Name);

            if (rp.Insert(reserveDto, reserverId))
                return StatusCode(200, new StandardResult { Success = true, Message = " Reserve created." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPatch, Authorize(Policy = "JustActive")]
        public IActionResult Update(UpdateReserveDto reserveDto)
        {
            if (rp.ExistForUpdate(reserveDto.ReserveDate, reserveDto.LocationId, reserveDto.Id))
                return StatusCode(500, new StandardResult { Success = false, Message = "you can't reserve this location on this date" });

            if (rp.Update(reserveDto))
                return StatusCode(200, new StandardResult<Reserve> { Success = true, Message = " Reserve updated.", Result = rp.GetById(reserveDto.Id) });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpDelete("{id}"), Authorize(Policy = "JustActive")]
        public IActionResult Delete(int id)
        {
            if (rp.Delete(id))
                return StatusCode(200, new StandardResult { Success = true, Message = " Reserve deleted." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }
    }
}
