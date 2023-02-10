using LocationService.Interface;
using LocationService.Model;
using LocationService.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daya.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepo lr;

        public LocationController(ILocationRepo lr)
        {
            this.lr = lr;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var all = lr.GetAll();

            if (all is null)
                return StatusCode(404, new StandardResult() { Success = true, Message = "there is no Location in database" });

            return StatusCode(200, new StandardResult<List<Locations>>() { Success = true, Message = "All Locations returned.", Result = all });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var locations = lr.GetById(id);
            if (locations is null)
                return StatusCode(404, new StandardResult() { Success = false, Message = "location not found ." });

            return StatusCode(200, new StandardResult<Locations>() { Success = true, Message = "location founded.", Result = locations });
        }

        [HttpPost, Authorize(Policy = "JustActive")]
        public IActionResult Create(AddLocationDto locationDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new StandardResult { Success = false, Message = "model not valid" });

            if (lr.Insert(locationDto, Guid.Parse(HttpContext.User.Identity.Name)))
                return StatusCode(200, new StandardResult { Success = true, Message = "location created." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPut, Authorize(Policy = "JustActive")]
        public IActionResult Update(UpdateLocationDto locationDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new StandardResult { Success = false, Message = "model not valid" });

            if (lr.Update(locationDto))
                return StatusCode(200, new StandardResult<Locations> { Success = true, Message = "location updated.", Result = lr.GetById(locationDto.Id) });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpDelete("{id}"), Authorize(Policy = "JustActive")]
        public IActionResult Delete(int id)
        {
            if (lr.Delete(id))
                return StatusCode(200, new StandardResult { Success = true, Message = "location deleted." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpGet, Authorize]
        public IActionResult Filter(int typeId , string? title)
        {
            var res = lr.FilterBy(typeId, title);
            if (res is null)
                return StatusCode(404, new StandardResult { Success = false, Message = "not found" });

            return StatusCode(200, new StandardResult<List<Locations>> { Success = true, Message = "Filtered.", Result = res });
        }
    }
}
