using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;


namespace QuickCampus_Core.Services
{
    public class RoleRepo : BaseRepository<BtprojecQuickcampustestContext, TblRole>, IRoleRepo
    {
        private readonly BtprojecQuickcampustestContext _context;
        private IConfiguration _config;

        public RoleRepo(BtprojecQuickcampustestContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<IGeneralResult<string>> AddRolePermissions(List<int> permissions,int RoleId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            try
            {
                foreach (var permission in permissions)
                {
                    if (_context.TblPermissions.Any(x => x.Id == permission))
                    {
                        TblRolePermission vm = new TblRolePermission
                        {
                            PermissionId = permission,
                            RoleId = RoleId
                        };
                        var addRolePermission = await _context.TblRolePermissions.AddAsync(vm);
                        _context.SaveChanges();
                        if (addRolePermission.Entity.Id == 0)
                        {
                            result.Message = "Something went wrong.";
                            return result;
                        }
                    }
                }
                result.IsSuccess = true;
                result.Message = "Permission saved successfully.";
                return result;
            }
            catch(Exception ex)
            {
                result.Message = "server error. " + ex.Message;
            }
            return result;
        }

        public async Task<IGeneralResult<string>> UpdateRolePermissions(List<int> permissions, int RoleId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            try
            {
                var LastRolePermissions = _context.TblRolePermissions.Where(x => x.RoleId == RoleId).ToList();
                _context.TblRolePermissions.RemoveRange(LastRolePermissions);
                _context.SaveChanges();

                foreach (var permission in permissions)
                {
                    if (_context.TblPermissions.Any(x => x.Id == permission))
                    {
                        TblRolePermission vm = new TblRolePermission
                        {
                            PermissionId = permission,
                            RoleId = RoleId
                        };
                        var addRolePermission = await _context.TblRolePermissions.AddAsync(vm);
                        _context.SaveChanges();
                        if (addRolePermission.Entity.Id == 0)
                        {
                            result.Message = "Something went wrong.";
                            return result;
                        }
                    }
                }
                result.IsSuccess = true;
                result.Message = "Permission saved successfully.";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = "server error. " + ex.Message;
            }
            return result;
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
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id==id && (clientid == 0 ? true : w.ClientId == clientid)).FirstOrDefault();
            }
            else
            {
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id == id && w.ClientId == clientid).FirstOrDefault();
            }
            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }
            rl.IsActive = isActive;
            rl.ModofiedDate = DateTime.Now;
            dbContext.TblRoles.Update(rl);
            int a = dbContext.SaveChanges();
            if (a > 0)
            {
                result.IsSuccess = true;
                result.Message = "Status update successfully";
                return result;
               
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "something went wrong";
                return result;
            }
        }


        public async Task<IGeneralResult<string>> DeleteRole(int id, int clientid, bool isSuperAdmin)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblRole rl = new TblRole();
            rl = _context.TblRoles.Find(id);

            if (rl == null)
            {
                result.IsSuccess = false;
                result.Message = "Role not found";
                return result;
            }
            rl.IsDeleted = true;
            rl.IsActive = false;
            rl.ModofiedDate = DateTime.Now;

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
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id==roleModel.Id && (clientid == 0 ? true : w.ClientId == clientid)).FirstOrDefault();
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
            rl.Id = roleModel.Id;
            rl.Name = roleModel.RoleName.Trim();
            rl.ModofiedDate = DateTime.Now;
           var data = dbContext.TblRoles.Update(rl);
            int a =await dbContext.SaveChangesAsync();
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
                rl = _context.TblRoles.Where(w => w.IsDeleted != true && w.Id == rId && (clientid == 0 ? true : w.ClientId == clientid)).FirstOrDefault();
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
