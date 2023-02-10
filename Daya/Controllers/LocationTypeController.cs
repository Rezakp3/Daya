using LocationService.Interface;
using LocationService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daya.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationTypeController : ControllerBase
    {
        private readonly ILocationTypeRepo ltr;

        public LocationTypeController(ILocationTypeRepo ltr)
        {
            this.ltr = ltr;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var all = ltr.GetAll();

            if (all is null)
                return StatusCode(404, new StandardResult() { Success = true, Message = "there is no Location type in database" });

            return StatusCode(200, new StandardResult<List<LocationType>>() { Success = true, Message = "All types returned .", Result = all });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var type = ltr.GetById(id);
            if (type is null)
                return StatusCode(404, new StandardResult() { Success = false, Message = "location type not found ." });

            return StatusCode(200, new StandardResult<LocationType>() { Success = true, Message = "location type founded .", Result = type });
        }

        [HttpPost, Authorize(Policy = "JustActive")]
        public IActionResult Create(string type)
        {
            if (ltr.Insert(type))
                return StatusCode(200, new StandardResult { Success = true, Message = "location type created." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpPatch, Authorize(Policy = "JustActive")]
        public IActionResult Update(LocationType type)
        {
            if (ltr.Update(type))
                return StatusCode(200, new StandardResult<LocationType> { Success = true, Message = "location type updated.", Result = ltr.GetById(type.Id) });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }

        [HttpDelete("{id}"), Authorize(Policy = "JustActive")]
        public IActionResult Delete(int id)
        {
            if (ltr.Delete(id))
                return StatusCode(200, new StandardResult { Success = true, Message = "location type deleted." });

            return StatusCode(500, new StandardResult { Success = false, Message = "Unknown Error." });
        }
    }
}
