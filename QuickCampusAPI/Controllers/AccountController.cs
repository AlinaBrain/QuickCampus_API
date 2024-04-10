using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using QuickCampus_Core.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuickCampus_Core.Common;
using Microsoft.Extensions.Options;
using QuickCampus_Core;
using QuickCampus_Core.Common.Helper;

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
        private readonly MailSettings _mailSettings;
        private readonly SendEmail _sendMail;
        private string _jwtSecretKey;

        public AccountController(IUserRepo userRepo, IConfiguration config, IAccount account, IOptions<MailSettings> mailSettings,SendEmail sendEmail)
        {
            _config = config;
            _mailSettings = mailSettings.Value;
            this.userRepo = userRepo;
            _account = account;
            _sendMail=sendEmail;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
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
        public async Task<IActionResult> GetAllPermission()
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
        [AllowAnonymous]
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string EmailId)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            var user = await _account.GetEmail(EmailId);
            if (user != null)
            {
                var token =  _account.GenerateTokenForgotPassword(EmailId,user.Id);
                user.ForgotPassword = token.ToString();
                await userRepo.Update(user);
                SendMailViewModel vm = new SendMailViewModel()
                {
                    ReceiverEmailId= EmailId,
                    Subject="Forget Password"
                };
                string body = "<h5>Hi #UserName#</h5><br/><p> Please #Link# to reset your password </p>";
                body = body.Replace("#UserName#", user.Name);
                var call = (Request.IsHttps ? "https://" : "http://") + _config["UIForgetPasswordUrl"] + "/#/Reset?passwordToken=" + token;
                var linkUrl = "<a href = '" + call + "'>click here</a>";
                body = body.Replace("#Link#", linkUrl);
                vm.Body = body;
                var sendmail = _sendMail.SendGridEmail(vm);
                if (sendmail.IsSuccess)
                {
                    return Ok(sendmail);
                }
                result.Message = sendmail.Message;
            }
            else
            {
                result.Message = "User not Found";
                return Ok(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromHeader]string passwordToken, ForgotPasswordVm vm)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            var UserId = JwtHelper.GetIdFromToken(Request.Headers["passwordToken"], _jwtSecretKey);
            var exptime = JwtHelper.GetExpiredTime(Request.Headers["passwordToken"], _jwtSecretKey);
            if (Convert.ToDateTime(exptime) > DateTime.Now)
            {
                var varifiedtoken = await _account.CheckToken(passwordToken, UserId);
                if (varifiedtoken != null)
                {
                    varifiedtoken.Password = CommonMethods.EncodePasswordToBase64( vm.Password);
                    varifiedtoken.ForgotPassword = "";
                    await userRepo.Update(varifiedtoken);
                    result.IsSuccess = true;
                    result.Message = "Password Updated Successfully";
                }
                else
                {
                    result.Message = "Invalid Token";
                }
            }
            else
            {
                result.Message = "Token Expired!";
            }

            return Ok(result);
        }

    }
}

