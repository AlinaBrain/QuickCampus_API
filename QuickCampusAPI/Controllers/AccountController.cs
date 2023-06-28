using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public AccountController(IApplicationUserRepo applicationUserRepo, IConfiguration config)
        {
            _config = config;
            _applicationUserRepo = applicationUserRepo;
        }
        [HttpPost]
        [Route("AdminLogin")]
        public IActionResult AdminLogin([FromBody] LoginVM loginVM)
        {
            var user = Authenticate(loginVM);
            if (user != null)
            {
                var token = Generate(loginVM);
                return Ok(token);

            }
            return NotFound("User Not Found");

        }

        private string Generate(LoginVM loginVM)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var Claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier,CommonMethods.ConvertToEncrypt(loginVM.UserName)),

                new Claim(ClaimTypes.Name,CommonMethods.ConvertToEncrypt(loginVM.Password))

            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
               _config["Jwt:Audience"],
               Claims,
               expires: DateTime.Now.AddHours(24),
               signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private ApplicationUserVM Authenticate(LoginVM loginVM)
        {
            var currentUser = _applicationUserRepo.FirstOrDefault(o => o.UserName.ToLower() == loginVM.UserName.ToLower() && o.Password == loginVM.Password);
            if (currentUser != null)
            {
                return (ApplicationUserVM)currentUser;
            }
            return null;
        }



      

    }
}
