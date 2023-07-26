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

        List<string> permissionRecord = new List<string>();

        public async Task<IGeneralResult<LoginResponseVM>> Login(AdminLogin adminLogin)
        {
            IGeneralResult<LoginResponseVM> response = new GeneralResult<LoginResponseVM>();
            LoginResponseVM data = new LoginResponseVM();
            response.Data = data;
            List<RoleMaster> rm = new List<RoleMaster>();

            adminLogin.Password = EncodePasswordToBase64(adminLogin.Password);
            response.Data.RoleMasters = rm;

            var re = _context.TblUsers.Include(i => i.TblUserRoles).Where(w => w.Email == adminLogin.UserName && w.Password == adminLogin.Password && w.IsDelete == false && w.IsActive == true).FirstOrDefault();
            
            if (re != null)
            {

                var user = _context.TblUserRoles.
                                Include(i => i.User)
                                .Include(i => i.Role)
                                .Include(i => i.Role.TblRolePermissions)
                                .Where(w => w.User.Email.ToLower() == adminLogin.UserName.ToLower() && w.User.Password == adminLogin.Password && w.User.IsDelete == false && w.User.IsActive == true)
                                .FirstOrDefault();

                var uRoles = _context.TblUserRoles.Include(w=>w.Role)
                    .Where(w => w.User.Email.ToLower() == adminLogin.UserName.ToLower() && w.User.Password == adminLogin.Password && w.User.IsDelete == false && w.User.IsActive == true)
                    .Select(s => new RoleMaster()
                    {
                        Id = s.Role.Id,
                        RoleName = s.Role.Name
                    }).ToList();

                response.Data.IsSuperAdmin = uRoles.Any(w => w.RoleName == "SuperAdmin");

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
                response.Data.Token = GenerateToken(adminLogin, record, re.ClientId == null ? 0 : re.ClientId, re.Id,response.Data.IsSuperAdmin);
                response.Data.UserName = re.UserName;
                response.Data.UserId = re.Id;
                response.Data.CilentId = re.ClientId;
               // response.Data.Createdby = re.Createdby;
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

            foreach (var rec in rolePermissions)
            {
                permissionRecord.Add(rec.PermissionName.Trim());
            }

            return rolePermissions;
        }

        private string GenerateToken(AdminLogin adminlogin, List<string> obj, int? clientId, int userId, bool isSuperAdmin)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
         {
                //new Claim(ClaimTypes.Name,clientId==0?string.Empty:clientId.ToString()),
                new Claim("UserId",userId.ToString()),
                new Claim("cilentId",clientId==0?string.Empty:clientId.ToString()),
                new Claim(ClaimTypes.Role,"Test"),
                new Claim("IsSuperAdmin",isSuperAdmin==true?"1":"0")
                //new Claim("IsSuperAdmin",(isSuperAdmin==true?"True":"False").ToString().Trim())
            };

           
            foreach (var role in permissionRecord)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
