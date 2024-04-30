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
using static QuickCampus_Core.Common.common;

namespace QuickCampusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepo userRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private IConfiguration _config;
        private readonly IAccount _account;
        private readonly MailSettings _mailSettings;
        private readonly SendEmail _sendMail;
        private string _jwtSecretKey;
        private readonly ProcessUploadFile _uploadFile;

        public AccountController(IUserRepo userRepo, IUserAppRoleRepo userAppRoleRepo, IConfiguration config, IAccount account, IOptions<MailSettings> mailSettings, SendEmail sendEmail, ProcessUploadFile uploadFile)
        {
            _config = config;
            _mailSettings = mailSettings.Value;
            this.userRepo = userRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _account = account;
            _sendMail = sendEmail;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _uploadFile = uploadFile;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> AdminLogin(AdminLogin AdminLogin)
        {
            var res = await _account.Login(AdminLogin);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllPermission")]
        public IActionResult GetAllPermission()
        {
            bool IsAdmin = false;
            var _jwtSecretKey = _config["Jwt:Key"];
            var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var LoggedInUserRole =  _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId)).Result.FirstOrDefault();
            if (LoggedInUserRole != null && (LoggedInUserRole.RoleId == (int)AppRole.Admin || LoggedInUserRole.RoleId == (int)AppRole.Admin_User)) IsAdmin = true;
                var res = _account.ListPermission(IsAdmin);
            return Ok(res);
        }

        //[HttpGet]
        //[Route("getallroles")]
        //public async Task<IActionResult> GetAllRoles()
        //{
        //    var _jwtSecretKey = _config["Jwt:Key"];
        //    var LoggedInUser = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
        //    var clientId = userRepo.GetAllQuerable().Where(x => x.Id.ToString() == LoggedInUser).Select(x => x.ClientId).First();
        //    var res = await _account.ListRoles(clientId ?? 0, Convert.ToInt32(LoggedInUser));
        //    return Ok(res);
        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string EmailId)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            var user = _account.GetEmail(EmailId);
            if (user != null)
            {
                var token = _account.GenerateTokenForgotPassword(EmailId, user.Id);
                user.ForgotPassword = token.ToString();
                await userRepo.Update(user);
                SendMailViewModel vm = new SendMailViewModel()
                {
                    ReceiverEmailId = EmailId,
                    Subject = "Forget Password"
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
        public async Task<IActionResult> ResetPassword([FromHeader] string passwordToken, ForgotPasswordVm vm)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            var UserId = JwtHelper.GetIdFromToken(Request.Headers["passwordToken"], _jwtSecretKey);
            var ExpiryTime = JwtHelper.GetExpiredTime(Request.Headers["passwordToken"], _jwtSecretKey);
            if (Convert.ToDateTime(ExpiryTime) > DateTime.Now)
            {
                var VerifiedToken = _account.CheckToken(passwordToken, UserId);
                if (VerifiedToken != null)
                {
                    VerifiedToken.Password = CommonMethods.EncodePasswordToBase64(vm.Password);
                    VerifiedToken.ForgotPassword = "";
                    await userRepo.Update(VerifiedToken);
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

        [HttpPost]
        [Route("ProfileChange")]
        public async Task<IActionResult> ProfileChange([FromForm]ProfileChangeVm vm )
        {
            IGeneralResult<ProfileChangeVm> result = new GeneralResult<ProfileChangeVm>();
            try
            {
                if (vm == null)
                {
                    result.Message = "Your Model request in Invalid";
                    return Ok(result);
                }
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                
                if (vm.Id > 0)
                {
                    var profile = _account.GetAllQuerable().Where(x => x.Id == vm.Id && x.IsDelete == false).FirstOrDefault();
                    if (profile != null)
                    {
                        profile.Name = vm.Name;
                        profile.Mobile = vm.Mobile;
                        profile.ModifiedDate = DateTime.Now;
                        
                        if (vm.ImagePath != null)
                        {
                            var CheckImg = _uploadFile.CheckImage(vm.ImagePath);
                            if (!CheckImg.IsSuccess)
                            {
                                result.Message = CheckImg.Message;
                                return Ok(result);
                            }
                        }
                        var UploadLogo = _uploadFile.GetUploadFile(vm.ImagePath);
                        if (UploadLogo.IsSuccess)
                        {
                            profile.ProfilePicture = UploadLogo.Data;
                            await _account.Update(profile);
                            result.IsSuccess = true;
                            result.Message = "Record Update Successfully";
                            result.Data = vm;
                            result.Data.ImagePath = null;
                            return Ok(result);
                        }
                    }
                    else
                    {
                        result.Message = "Something went wrong.";
                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "server error. " + ex.Message;
            }
            return Ok(result);
        }
    }
}

