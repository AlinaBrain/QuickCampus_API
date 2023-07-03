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
                var uRoles = _context.TblUserRoles
                    .Where(w => w.User.UserName == adminLogin.UserName && w.User.Password == adminLogin.Password && w.User.IsDelete == false && w.User.IsActive == true)
                    .Select(s => new RoleMaster()
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

            rolePermissions = _context.TblRolePermissions.Include(i => i.Permission).Where(w => w.RoleId == roleId).Select(s => new RolePermissions()
            {
                Id = s.Id,
                PermissionName = s.Permission.PermissionName,
                DisplayName = s.Permission.PermissionDisplay
            }).ToList();

            return rolePermissions;
        }

        private string GenerateToken(AdminLogin adminlogin, string userRole, List<string> obj)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var Claims = new[]
         {
                new Claim(ClaimTypes.Name,adminlogin.UserName),
                new Claim(ClaimTypes.Role,"Admin")
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
               _config["Jwt:Audience"],
               Claims,
               expires: DateTime.Now.AddHours(24),
               signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IGeneralResult<List<PermissionVM>>> ListPermission()
        {
            IGeneralResult<List<PermissionVM>> lst = new GeneralResult<List<PermissionVM>>();
            lst.Data = new List<PermissionVM>();

            var record = await _context.TblPermissions.Select(s => new PermissionVM()
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

        public async Task<IGeneralResult<List<RoleMappingVM>>> ListRoles()
        {
            IGeneralResult<List<RoleMappingVM>> lst = new GeneralResult<List<RoleMappingVM>>();
            lst.Data = new List<RoleMappingVM>();
            var record = await _context.TblRoles.Select(s => new RoleMappingVM()
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
    }
}
