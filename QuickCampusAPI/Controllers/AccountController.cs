using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IApplicationUserRepo _applicationUserRepo;
        private IConfiguration _config;
        private readonly IAccount _account;
        public AccountController(IApplicationUserRepo applicationUserRepo, IConfiguration config, IAccount account)
        {
            _config = config;
            _applicationUserRepo = applicationUserRepo;
            _account = account;
        }
        [AllowAnonymous]
        [HttpPost]  
        [Route("AdminLogin")]
        public IActionResult AdminLogin(AdminLogin adminlogin)
        {
            var res = _account.Login(adminlogin);
            return Ok(res);
        }

        [HttpGet]
        [Route("getallpermission")]
        public async  Task<IActionResult> GetAllPermission()
        {
            var res = await _account.ListPermission();
            return Ok(res);
        }
        
        [HttpGet]
        [Route("getallroles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var res = await _account.ListRoles();
            return Ok(res);
        }
        private ApplicationUserVM Authenticate(AdminLogin adminLogin)
        {
            var currentUser = _applicationUserRepo.FirstOrDefault(o => o.UserName.ToLower() == adminLogin.UserName.ToLower() && o.Password == adminLogin.Password);
            if (currentUser != null)
            {
                return (ApplicationUserVM)currentUser;
            }
            return null;
        }
    }
}
