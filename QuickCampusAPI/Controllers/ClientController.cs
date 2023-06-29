using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ClientController : ControllerBase
    {
        [Route("GetRecord")]
        [HttpGet]
        public IActionResult getrecord()
        {
            return Ok("Hi Krishan kumar");
        }
    }
}

