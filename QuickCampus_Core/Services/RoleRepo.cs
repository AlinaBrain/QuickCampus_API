using Microsoft.Extensions.Configuration;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;


namespace QuickCampus_Core.Services
{
    public class RoleRepo :BaseRepository<QuikCampusDevContext, TblRole>, IRoleRepo
    {
        private readonly QuikCampusDevContext _context;
        private IConfiguration _config;

        public RoleRepo (QuikCampusDevContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<string> SetRolePermission(RoleMappingRequest roleMappingRequest)
        {

            var roleIds = _context.TblRoles.Select(s => s.Id).ToList();
            var permissionIds = _context.TblPermissions.Select(s => s.Id).ToList();

            var rolePermissions = _context.TblRolePermissions.ToList();

            foreach (var rIds in roleMappingRequest.roleIds)
            {
                bool isRoleIdExist = roleIds.Contains(rIds.RoleId);
                if (!isRoleIdExist)
                {
                    break ;
                }
                foreach (var pId in rIds.PermissionIds)
                {
                    bool isPermissionIdExist = permissionIds.Contains(pId);
                    if (!isPermissionIdExist)
                    {
                        break;
                    }

                    int count = rolePermissions.Where(w => w.PermissionId == pId && w.RoleId == rIds.RoleId).Count(); 
                    
                    if(count > 0)
                    {
                        break;
                    }
                    


                    TblRolePermission record = new TblRolePermission()
                    {
                        PermissionId = pId,
                        RoleId = rIds.RoleId,
                        PermissionName = "",
                        DisplayName = ""
                    };
                    _context.TblRolePermissions.Add(record);
                }
            }
            int save =  _context.SaveChanges();
            return "SetRolePermissions";
        }
    }
}
