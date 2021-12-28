using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            var result = User.Claims.Select(i => new { i.Type, i.Value });
            return new JsonResult(result);
        }

    }
}
