using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Data;
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

            List<RoleMaster> rm = new List<RoleMaster>();

            response.Data.RoleMasters = rm;

            var user = _context.TblUserRoles.
                     Include(i => i.User)
                     .Include(i => i.Role)
                     .Include(i => i.Role.TblRolePermissions)
                     .Where(w => w.User.UserName == adminLogin.UserName && w.User.Password == adminLogin.Password && w.User.IsDelete == false && w.User.IsActive == true)
                     .FirstOrDefault();

            if (user != null)
            {


                var uRoles = _context.TblUserRoles.Where(w => w.User.UserName == adminLogin.UserName && w.User.Password == adminLogin.Password && w.User.IsDelete == false && w.User.IsActive == true).Select(s => new RoleMaster()
                {
                    Id = s.Role.Id,
                    RoleName = s.Role.Name
                }).ToList();

                foreach (var rec in uRoles)
                {
                    response.Data.RoleMasters.Add(new RoleMaster()
                    {
                        Id = rec.Id,
                        RoleName = rec.RoleName,
                        rolePermissions = getPermission(rec.Id, user.Role)
                    });
                }

                response.IsSuccess = true;
                response.Message = "Login Successuflly";
                List<string> record = new List<string>();
                record = uRoles.Select(s => s.RoleName).ToList();
                response.Data.Token = GenerateToken(adminLogin, user.Role.Name, record);
                response.Data.UserName = user.User.UserName;
                response.Data.UserId = user.Id;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "User Not Found";
            }
            return response;
        }


        public List<RolePermissions> getPermission(int roleId, TblRole tblRole)
        {
            List<RolePermissions> rolePermissions = new List<RolePermissions>();

            rolePermissions = tblRole.TblRolePermissions.Where(w => w.RoleId == roleId).Select(s => new RolePermissions()
            {
                Id = s.Id,
                PermissionName = s.PermissionName,
                DisplayName = s.DisplayName
            }).ToList();

            return rolePermissions;
        }

        private string GenerateToken(AdminLogin adminlogin, string userRole, List<string> obj)
        {



            // Create claims for each role
            List<Claim> roleClaims = obj.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            // Create other claims as needed
            List<Claim> otherClaims = new List<Claim>
                {
                    // Add additional claims if necessary
                    new Claim("UserName", adminlogin.UserName),
                    new Claim("Password", adminlogin.Password)
                };

            // Combine all claims
            List<Claim> allClaims = new List<Claim>();
            allClaims.AddRange(roleClaims);
            allClaims.AddRange(otherClaims);

            // Create a JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]); // Replace with your secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(allClaims),
                Expires = DateTime.UtcNow.AddDays(7), // Set token expiration as per your requirements
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token1 = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token1);

            return jwtToken;
        }
    }
}
