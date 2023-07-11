using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
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
