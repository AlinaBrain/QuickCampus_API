using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickCampusAPI.Controllers
{
    [Authorize(Roles = "a,b")]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [Route("rec")]
        [HttpGet]
        public IActionResult record()
        {
            return Ok("Record");
        }
    }
}
