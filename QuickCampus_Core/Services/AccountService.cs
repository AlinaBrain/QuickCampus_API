using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuickCampus_Core.Services
{
    public class AccountService : IAccount
    {

        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;
        public AccountService(QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin)
        {
            IGeneralResult<LoginResponseVM> response = new GeneralResult<LoginResponseVM>();
            LoginResponseVM data = new LoginResponseVM();
            response.Data = data;
            var currentUser = _context.TblUserRoles.
                Include(i=>i.User)
                .Include(i=>i.Role)
                .Include(i=>i.Role.TblRolePermissions)
                .Where(w => w.User.UserName == adminLogin.UserName && w.User.Password == adminLogin.Password)
                .FirstOrDefault();



            if (currentUser != null)
            {
                response.IsSuccess = true;
                response.Message = "Login Successuflly";
                response.Data.Token = GenerateToken(adminLogin, currentUser.Role.Name);
                response.Data.rolePermissions = currentUser.Role.TblRolePermissions.Select(s => new RolePermissions()
                {
                    Id = s.Id,
                    DisplayName = s.DisplayName,
                    PermissionName = s.PermissionName
                }).ToList();


            }
            else
            {
                response.IsSuccess = false;
                response.Message = "User Not Found";
            }
            return response;
        }


        private string GenerateToken(AdminLogin adminlogin,string userRole)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var Claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier,CommonMethods.ConvertToEncrypt(adminlogin.UserName)),
                new Claim(ClaimTypes.Name,CommonMethods.ConvertToEncrypt(adminlogin.Password)),
                new Claim(ClaimTypes.Role,CommonMethods.ConvertToEncrypt(userRole))
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
               _config["Jwt:Audience"],
               Claims,
               expires: DateTime.Now.AddHours(24),
               signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
