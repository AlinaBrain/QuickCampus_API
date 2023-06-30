using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepo roleRepo;
        private readonly IUserRepo userRepo;
        public RoleController(IRoleRepo roleRepo, IUserRepo userRepo)
        {
            this.roleRepo = roleRepo;
            this.userRepo = userRepo;
        }
        [HttpPost]
        [Route("roleAdd")]
        public async Task<IActionResult> roleAdd([FromBody] RoleModel vm)
        {
            IGeneralResult<RoleVm> result = new GeneralResult<RoleVm>();
            if (ModelState.IsValid)
            {

                var user = await userRepo.GetById(vm.userId);
                RoleVm roleVm = new RoleVm
                {
                    Name = vm.RoleName,
                    CreatedBy = user.Id,
                    ModifiedBy = user.Id,
                    CreatedDate = DateTime.Now,

                };
                await roleRepo.Add(roleVm.toRoleDBModel());
                result.Message = "Role added successfully";
                result.IsSuccess = true;
                result.Data = roleVm;
                return Ok(result);
            }
            else
            {
                result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("roleList")]
        public async Task<IActionResult> roleList()
        {
            List<RoleVm> roleVm = new List<RoleVm>();
            var rolelist = (await roleRepo.GetAll()).ToList();
            roleVm = rolelist.Select(x => ((RoleVm)x)).ToList();
            return Ok(roleVm);
        }

        [HttpPost]
        [Route("roleEdit")]
        public async Task<IActionResult> Edit(int roleId, RoleModel vm)
        {
            IGeneralResult<RoleVm> result = new GeneralResult<RoleVm>();
            var uId = await userRepo.GetById(vm.userId);
            var res = await roleRepo.GetById(roleId);
            if (res != null)
            {
                res.Id = roleId;
                res.Name = vm.RoleName;
                res.ModifiedBy = vm.userId;
                res.ModofiedDate = DateTime.Now;
                await roleRepo.Update(res);
                result.Message = "Role data is updated successfully";
                result.IsSuccess = true;
                result.Data = (RoleVm)res;
                return Ok(result);
            }
            else
            {
                result.Message = GetErrorListFromModelState.GetErrorList(ModelState);
            }
            return Ok(result);
        }
    }
}
