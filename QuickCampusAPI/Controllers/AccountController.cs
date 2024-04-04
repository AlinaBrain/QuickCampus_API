using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using QuickCampus_Core.Services;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepo userRepo;
        private IConfiguration _config;
        private readonly IAccount _account;
        public AccountController(IUserRepo userRepo, IConfiguration config, IAccount account)
        {
            _config = config;
           
            this.userRepo = userRepo;
            _account = account;
        }
        [AllowAnonymous]
        [HttpPost]  
        [Route("Login")]
        public async Task<IActionResult> AdminLogin(AdminLogin adminlogin)
        {
            var res = await _account.Login(adminlogin);
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
            var _jwtSecretKey = _config["Jwt:Key"];
            var LoggedInUser = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var clientId = userRepo.GetAllQuerable().Where(x => x.Id.ToString() == LoggedInUser).Select(x => x.ClientId).First();
            var res = await _account.ListRoles(clientId ?? 0, Convert.ToInt32(LoggedInUser));
            return Ok(res);
        }
        //private ApplicationUserVM Authenticate(AdminLogin adminLogin)
        //{
        //    var currentUser = _applicationUserRepo.FirstOrDefault(o => o.UserName.ToLower() == adminLogin.UserName.ToLower() && o.Password == adminLogin.Password);
        //    if (currentUser != null)
        //    {
        //        return (ApplicationUserVM)currentUser;
        //    }
        //    return null;
        //}
    }
}
