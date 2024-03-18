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

            adminLogin.Password = EncodePasswordToBase64(adminLogin.Password);

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
                response.Data.CilentId = findUser.ClientId ;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "User Not Found";
            }
            return response;
        }

        private List<RolePermissions> GetUserPermission(int RoleId)
        {
            List<RolePermissions> rolePermissions = new List<RolePermissions>();

            rolePermissions = _context.TblRolePermissions.Include(i => i.Permission).Where(w => w.RoleId == RoleId).Select(s => new RolePermissions()
            {
                Id = s.Id,
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
                new Claim("RolesArray",roleArr ?? ""),
                new Claim("UserAppRole",roleVm.UserAppRoleName ?? ""),
                new Claim(ClaimTypes.Role,roleVm.RoleName),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
               expires: DateTime.Now.AddHours(24), signingCredentials: credentials);
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

        public async Task<IGeneralResult<List<RoleMappingVM>>> ListRoles(int ClientId, int UserId)
        {
            IGeneralResult<List<RoleMappingVM>> lst = new GeneralResult<List<RoleMappingVM>>();
            lst.Data = new List<RoleMappingVM>();
            var record = await _context.TblRoles.Where(x=>x.IsDeleted == false && x.IsActive == true &&  (ClientId > 0 ? x.ClientId == ClientId : x.CreatedBy == UserId)).Select(s => new RoleMappingVM()
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
        
        private string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
    }
}
