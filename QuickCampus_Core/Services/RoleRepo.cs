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

        public async Task<IGeneralResult<string>> AddRolePermissions(List<int> permissions, int RoleId)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            try
            {
                foreach (var permission in permissions)
                {
                    if (_context.MstPermissions.Any(x => x.Id == permission))
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
                    if (_context.MstPermissions.Any(x => x.Id == permission))
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

    }
}
