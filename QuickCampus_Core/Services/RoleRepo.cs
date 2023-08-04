using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;


namespace QuickCampus_Core.Services
{
    public class RoleRepo : BaseRepository<QuikCampusDevContext, TblRole>, IRoleRepo
    {
        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;

        public RoleRepo(QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<IGeneralResult<string>> SetRolePermission(RoleMappingRequest roleMappingRequest)
        {
            IGeneralResult<string> result = new GeneralResult<string>();

            var roleIds = _context.TblRoles.Select(s => s.Id).ToList();
            var permissionIds = _context.TblPermissions.Select(s => s.Id).ToList();
            var assignPermission = _context.TblRolePermissions.Where(w => w.RoleId == roleMappingRequest.RoleId).ToList();

            bool isRoleIdExist = roleIds.Contains(roleMappingRequest.RoleId);
            if (!isRoleIdExist)
            {
                result.IsSuccess = false;
                result.Message = "RoleId does not exist.";
                result.Data = null;
                return result;
            }


            if (roleMappingRequest.RoleId > 0)
            {
                _context.TblRolePermissions.RemoveRange(assignPermission);
            }


            foreach (var pId in roleMappingRequest.permissions)
            {
                bool isPermissionIdExist = permissionIds.Contains(pId.PermissionIds);
                if (!isPermissionIdExist)
                {
                    result.IsSuccess = false;
                    result.Message = "Permissionid does not exist.";
                    result.Data = null;
                    return result;
                }

                TblRolePermission record = new TblRolePermission()
                {
                    PermissionId = pId.PermissionIds,
                    RoleId = roleMappingRequest.RoleId,
                    PermissionName = "",
                    DisplayName = ""
                };
                _context.TblRolePermissions.Add(record);

            }
            int save = _context.SaveChanges();

            if (save > 0)
            {
                result.IsSuccess = true;
                result.Message = "Permission assigned successfully.";
                result.Data = null;
                return result;
            }
            return result;
        }

        public async Task<IGeneralResult<string>> ActiveInActiveRole(bool isActive, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblRole rl = new TblRole();
            if (isSuperAdmin)
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && clientid == 0 ? true : w.ClientId == clientid).FirstOrDefault();
            }
            else
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.ClientId == clientid).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }

            rl.IsActive = isActive;
            dbContext.TblRoles.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "status update successfully";
                return result;
               
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }


        public async Task<IGeneralResult<string>> DeleteRole(bool isDeleted, int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblRole rl = new TblRole();
            if (isSuperAdmin)
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && clientid == 0 ? true : w.ClientId == clientid).FirstOrDefault();
            }
            else
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.ClientId == clientid).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }

            rl.IsDeleted = isDeleted;
            dbContext.TblRoles.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "Role delete successfully";
                return result;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }

        public async Task<IGeneralResult<string>> UpdateRole(RoleModel roleModel, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblRole rl = new TblRole();
            if (isSuperAdmin)
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id==roleModel.Id && clientid == 0 ? true : w.ClientId == clientid).FirstOrDefault();
            }
            else
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id == roleModel.Id && w.ClientId == clientid).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }

            rl.Name = roleModel.RoleName;
            rl.ModofiedDate = DateTime.Now;
            dbContext.TblRoles.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "Role update successfully";
                return result;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }


        public async Task<IGeneralResult<RoleModel>> GetRoleById(int rId, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<RoleModel> result = new GeneralResult<RoleModel>();
            result.Data = new RoleModel();
            TblRole rl = new TblRole();
            if (isSuperAdmin)
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id == rId && clientid == 0 ? true : w.ClientId == clientid).FirstOrDefault();
            }
            else
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id == rId && w.ClientId == clientid).FirstOrDefault();
            }
            if (rl != null)
            {
                RoleModel rlm = new RoleModel()
                {
                    Id = rl.Id,
                    RoleName = rl.Name
                };

                result.IsSuccess = true;
                result.Message = "Role fetch successfully";
                result.Data = rlm;
                return result;
            }

            else
            {
                result.IsSuccess = false;
                result.Message = "no record found";
                result.Data = null;
                return result;
            }
           
        }
    }
}
