using Microsoft.AspNetCore.Mvc;

namespace QuickCampusAPI.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
