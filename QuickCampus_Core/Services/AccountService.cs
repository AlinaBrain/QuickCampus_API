﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace QuickCampus_Core.Services
{
    public class AccountService : IAccount
    {

        private readonly BtprojecQuickcampustestContext _context;
        private IConfiguration _config;
        public AccountService(BtprojecQuickcampustestContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin)
        {
            IGeneralResult<LoginResponseVM> response = new GeneralResult<LoginResponseVM>();
            LoginResponseVM data = new LoginResponseVM();
            response.Data = data;

            adminLogin.Password = CommonMethods.EncodePasswordToBase64(adminLogin.Password);

            var findUser = _context.TblUsers.Include(i => i.TblUserRoles).Where(w => w.Email == adminLogin.UserName && w.Password == adminLogin.Password.ToLower() && w.IsDelete == false && w.IsActive == true).FirstOrDefault();

            if (findUser != null)
            {
                var uRoles = await _context.TblUserRoles
                    .Include(w => w.Role)
                    .Where(w => w.UserId == findUser.Id)
                    .FirstOrDefaultAsync();

                var uAppRole = await _context.TblUserAppRoles.Where(x => x.UserId == findUser.Id).FirstOrDefaultAsync();

                response.Data.RoleMasters = new RoleMaster()
                {
                    Id = uRoles.Id,
                    RoleName = uRoles.Role.Name,
                    UserAppRoleName = uAppRole != null ? ((common.AppRole)uAppRole.RoleId).ToString() : "",
                    rolePermissions = GetUserPermission(uRoles.RoleId ?? 0)
                };
                response.IsSuccess = true;
                response.Message = "Login Successfully";
                response.Data.Token = GenerateToken(adminLogin, response.Data.RoleMasters, findUser.ClientId == null ? 0 : findUser.ClientId, findUser.Id, response.Data.IsSuperAdmin);
                response.Data.UserName = findUser.Email;
                response.Data.UserId = findUser.Id;
                response.Data.CilentId = findUser.ClientId;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "UserName or Password is Incorrect";
            }
            return response;
        }

        public List<RolePermissions> GetUserPermission(int RoleId)
        {
            List<RolePermissions> rolePermissions = new List<RolePermissions>();

            //var UserPermission = _context.TblMenuItemUserPermissions.Where(x => x.UserId == UserId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

            rolePermissions = _context.TblRolePermissions.Include(i => i.Permission).Where(w => w.RoleId == RoleId).Select(s => new RolePermissions()
            {
                Id = s.PermissionId ?? 0, 
                PermissionName = s.Permission.PermissionName,
                DisplayName = s.Permission.PermissionDisplay
            }).ToList();
            return rolePermissions;
        }

        private string GenerateToken(AdminLogin adminlogin, RoleMaster roleVm, int? clientId, int userId, bool isSuperAdmin)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            string roleArr = JsonSerializer.Serialize(roleVm.rolePermissions);
            var claims = new List<Claim>
         {
                new Claim("UserId",userId.ToString()),
                new Claim("ClientId",clientId.ToString() ?? "0"),
                //new Claim("RolesArray",roleArr ?? ""),
                new Claim("UserAppRole",roleVm.UserAppRoleName ?? ""),
                new Claim(ClaimTypes.Role,roleVm.UserAppRoleName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
               expires: DateTime.Now.AddHours(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IGeneralResult<List<PermissionVM>>> ListPermission()
        {
            IGeneralResult<List<PermissionVM>> lst = new GeneralResult<List<PermissionVM>>();
            lst.Data = new List<PermissionVM>();

            var record = await _context.MstPermissions.Select(s => new PermissionVM()
            {
                Id = s.Id,
                PermissionDisplay = s.PermissionDisplay,
                PermissionName = s.PermissionName
            }).ToListAsync();

            if (record.Count != 0)
            {
                lst.Message = "Permissions";
                lst.IsSuccess = true;
                lst.Data = record;
            }
            else
            {
                lst.Message = "No Record Found";
                lst.IsSuccess = false;
            }
            return lst;
        }

        public async Task<IGeneralResult<List<RoleMappingVM>>> ListRoles(int ClientId, int UserId)
        {
            IGeneralResult<List<RoleMappingVM>> lst = new GeneralResult<List<RoleMappingVM>>();
            lst.Data = new List<RoleMappingVM>();
            var record = await _context.TblRoles.Where(x => x.IsDeleted == false && x.IsActive == true && (ClientId > 0 ? x.ClientId == ClientId : x.CreatedBy == UserId)).Select(s => new RoleMappingVM()
            {
                Id = s.Id,
                RoleName = s.Name
            }).ToListAsync();

            if (record.Count > 0)
            {
                lst.Message = "Role List";
                lst.IsSuccess = true;
                lst.Data = record;
            }
            else
            {
                lst.Message = "No Record Found";
                lst.IsSuccess = true;
            }
            return lst;
        }

        public async Task<IGeneralResult<RoleMappingVM>> GetRolePermissionByRoleIds(int[] roleIds)
        {
            var a = new List<int>();
            foreach (int id in roleIds)
            {
                a.Add(id);
            }

            GeneralResult<RoleMappingVM> response = new GeneralResult<RoleMappingVM>();

            //var rec= _context.TblRolePermissions.Where(w=> a.Contains(w.RoleId)).

            return response;
        }

        

        public async Task<TblUser> GetEmail(string emailId)
        {
            var result = _context.TblUsers.Where(s => s.Email == emailId && s.IsActive == true && s.IsDelete == false).FirstOrDefault();
            return result;
        }
        public string GenerateTokenForgotPassword(string EmailId ,int userId)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            int passwordExpiredTime = Convert.ToInt32((_config["Jwt:PasswordExpired"]));
            var claims = new List<Claim>
         {
                new Claim("UserId",userId.ToString()),
                new Claim("EmailId",EmailId.ToString()),
                new Claim("ExpiredOn",DateTime.Now.AddMinutes(passwordExpiredTime).ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
               expires: DateTime.Now.AddMinutes(passwordExpiredTime), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<TblUser> CheckToken(string token,string userid)
        {
            var result = _context.TblUsers.Where(s=>s.ForgotPassword ==token && s.IsActive==true && s.IsDelete == false && s.Id.ToString()==userid).FirstOrDefault();
            return result;
        }
    }
}
