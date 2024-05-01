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
        private readonly ITemplateRepo templateRepo;
        private readonly IUserAppRoleRepo _userAppRoleRepo;
        private IConfiguration _config;
        private readonly IAccount _account;
        private readonly MailSettings _mailSettings;
        private readonly SendEmail _sendMail;
        private string _jwtSecretKey;
        private readonly ProcessUploadFile _uploadFile;
       
        public AccountController(IUserRepo userRepo, ITemplateRepo templateRepo, IUserAppRoleRepo userAppRoleRepo, IConfiguration config, IAccount account, IOptions<MailSettings> mailSettings, SendEmail sendEmail, ProcessUploadFile uploadFile)
        {
            _config = config;
            _mailSettings = mailSettings.Value;
            this.userRepo = userRepo;
            this.templateRepo = templateRepo;
            _userAppRoleRepo = userAppRoleRepo;
            _account = account;
            _sendMail = sendEmail;
            _jwtSecretKey = _config["Jwt:Key"] ?? "";
            _uploadFile = uploadFile;

        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AdminLogin AdminLogin)
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

        [HttpGet]
        [Route("GetUserMenu")]
        public IActionResult GetUserMenu()
        {
            var _jwtSecretKey = _config["Jwt:Key"];
            var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            var result = _account.GetUserMenu(Convert.ToInt32(LoggedInUserId));
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string EmailId)
        {
            IGeneralResult<dynamic> result = new GeneralResult<dynamic>();
            var user = _account.GetEmail(EmailId);
            if (user != null)
            {
                int passwordExpiredTime = Convert.ToInt32((_config["Jwt:PasswordExpired"]));
                DateTime PasswordExpiry = DateTime.Now.AddMinutes(passwordExpiredTime);
                var token = _account.GenerateTokenForgotPassword(EmailId, user.Id, PasswordExpiry);
                user.ForgotPassword = token.ToString();
                await userRepo.Update(user);
                var ForgotPasswordTemplate = templateRepo.GetAllQuerable().Where(x => x.Subject == "Forgot Password" && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();

                SendMailViewModel vm = new SendMailViewModel()
                {
                    ReceiverEmailId = EmailId,
                    Subject = ForgotPasswordTemplate?.Subject ?? ""
                };

                string body = ForgotPasswordTemplate?.Body ?? "";
                var url = _config["UIForgetPasswordUrl"] + "?passwordToken=" + token;

                body = body.Replace("{{name}}", user.Name);
                body = body.Replace("{{action_url}}", url);
                body = body.Replace("{{expiry_time}}", PasswordExpiry.ToString());

                vm.Body = body;

                var IsMailSent = _sendMail.SendGridEmail(vm);
                if (IsMailSent.IsSuccess)
                {
                    return Ok(IsMailSent);
                }
                result.Message = IsMailSent.Message;
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
        [Route("VerifyForgetPassword")]
        public async Task<IActionResult> VerifyForgetPassword([FromHeader] string passwordToken, ForgotPasswordVm vm)
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
                
                
                    var profile = _account.GetAllQuerable().Where(x => x.Id ==Convert.ToInt32(LoggedInUserId) && x.IsDelete == false).FirstOrDefault();
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
            catch (Exception ex)
            {
                result.Message = "server error. " + ex.Message;
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordChangeVm vm)
        {
            IGeneralResult<PasswordChangeVm> result=new GeneralResult<PasswordChangeVm>();
            try
            {
                var LoggedInUserId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserClientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
                var LoggedInUserRole = (await _userAppRoleRepo.GetAll(x => x.UserId == Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if (LoggedInUserClientId == null || LoggedInUserClientId == "0")
                {
                    var user = await userRepo.GetById(Convert.ToInt32(LoggedInUserId));
                    LoggedInUserClientId = (user.ClientId == null ? "0" : user.ClientId.ToString());
                }
                var passwordCheck = (await _account.GetAll(x => x.Password == CommonMethods.EncodePasswordToBase64(vm.OldPassWord) && x.Id== Convert.ToInt32(LoggedInUserId))).FirstOrDefault();
                if(passwordCheck != null)
                {
                    passwordCheck.Password = CommonMethods.EncodePasswordToBase64(vm.PassWord);
                    passwordCheck.ModifiedDate= DateTime.Now;
                    await _account.Update(passwordCheck);
                    result.IsSuccess = true;
                    result.Message = "Password Change Successfully";
                }
                else
                {
                    result.Message = "Incorrect Password";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                 return Ok(result);
            }

            return Ok(result);
        }
    }
}

