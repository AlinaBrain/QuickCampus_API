using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    //[HttpPost]
    //public IActionResult Add(UserVm vm)
    //{
    //    return Ok(vm);
    //}
}
