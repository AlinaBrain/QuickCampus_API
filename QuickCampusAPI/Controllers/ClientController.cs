using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

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

