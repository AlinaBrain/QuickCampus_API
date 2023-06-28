using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo userRepo;
        public UserController(IUserRepo userRepo)
        {
            this.userRepo = userRepo; 
            
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("userAdd")]
        public async Task<IActionResult> addUser(UserModel vm)
        {
            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
            UserVm userVm = new UserVm
            {
                UserName = vm.UserName,
                Name = vm.Name,
                Email = vm.Email,
                Mobile = vm.Mobile,
                Password = vm.Password,
                IsActive =true,
                IsDelete = false,
            };
            if (ModelState.IsValid)
            {
                await userRepo.Add(userVm.toUserDBModel());
            }
            return Ok(userVm);
        }
        [HttpGet]
        [Route("userList")]
        public async Task<IActionResult> userList()
        {
            List<UserVm> vm = new List<UserVm>();
            var list = (await userRepo.GetAll()).Where(x => x.IsDelete == false && x.IsActive == true).ToList();
            vm = list.Select(x => ((UserVm)x)).ToList();
            return Ok(vm);
        }
        [HttpGet]
        [Route("userDelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await userRepo.GetById(id);
            if (res != null)
            {
                res.IsActive = false;
                res.IsDelete = true;
                await userRepo.Update(res);
                return Ok(res);
            }
            return Ok();
        }

    }
}
