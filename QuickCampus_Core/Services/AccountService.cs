using Microsoft.EntityFrameworkCore;
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
                response.Data.Token = GenerateToken(adminLogin, response.Data.RoleMasters, findUser.ClientId == null ? 0 : findUser.ClientId, findUser.Id);
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
                PermissionName = s.Permission.SubItemName,
                DisplayName = s.Permission.SubItemDisplayName
            }).ToList();
            return rolePermissions;
        }

        public IGeneralResult<List<RolesItemVm>> ListPermission(bool IsAdmin)
        {
            IGeneralResult<List<RolesItemVm>> rolesData = new GeneralResult<List<RolesItemVm>>();
            var rolesList = _context.MstMenuItems.Include(x => x.MstMenuSubItems).Where(y => y.IsActive == true && (!IsAdmin ? !y.ItemName.Contains("Client") : true)).Select(z => new RolesItemVm
            {
                ItmeId = z.ItemId,
                ItmeIcon = z.ItemIcon,
                ItmeName = z.ItemDisplayName,
                ItemSubMenu = z.MstMenuSubItems.Select(u => new PermissionVM
                {
                    Id = u.SubItemId,
                    PermissionDisplay = u.SubItemIcon,
                    PermissionName = u.SubItemDisplayName
                }).ToList()
            }).ToList();

            //var record = await _context.MstPermissions.Select(s => new PermissionVM()
            //{
            //    Id = s.Id,
            //    PermissionDisplay = s.PermissionDisplay,
            //    PermissionName = s.PermissionName
            //}).ToListAsync();

            if (rolesList.Count > 0)
            {
                rolesData.Message = "Permissions fetched successfully";
                rolesData.IsSuccess = true;
                rolesData.Data = rolesList;
            }
            else
            {
                rolesData.Message = "No Record Found";
                rolesData.IsSuccess = false;
            }
            return rolesData;
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

        public string GenerateTokenForgotPassword(string EmailId, int UserId)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            int passwordExpiredTime = Convert.ToInt32((_config["Jwt:PasswordExpired"]));
            var claims = new List<Claim>
         {
                new Claim("UserId",UserId.ToString()),
                new Claim("EmailId",EmailId.ToString()),
                new Claim("ExpiredOn",DateTime.Now.AddMinutes(passwordExpiredTime).ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
               expires: DateTime.Now.AddMinutes(passwordExpiredTime), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TblUser CheckToken(string token, string UserId)
        {
            var result = _context.TblUsers.Where(s => s.ForgotPassword == token && s.IsActive == true && s.IsDelete == false && s.Id.ToString() == UserId).FirstOrDefault();
            return result;
        }
        public TblUser GetEmail(string emailId)
        {
            var result = _context.TblUsers.Where(s => s.Email == emailId && s.IsActive == true && s.IsDelete == false).FirstOrDefault();
            return result;
        }

        private string GenerateToken(AdminLogin AdminLogin, RoleMaster roleVm, int? clientId, int userId)
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
            foreach (var role in roleVm.rolePermissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.PermissionName));
            }


            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
           expires: DateTime.Now.AddHours(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
