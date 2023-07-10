using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;

namespace QuickCampusAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class ClientController : ControllerBase
    {
        private readonly IClientRepo _clientRepo;
        private readonly IUserRepo userRepo;
        private IConfiguration config;
        public ClientController(IClientRepo clientRepo, IConfiguration config, IUserRepo userRepo)
        {
            _clientRepo = clientRepo;
            this.config = config;
            this.userRepo = userRepo;
        }

        [HttpGet]
        [Route("ClientList")]
        public async Task<IActionResult> ClientList()
        {


            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
            List<UserVm> vm = new List<UserVm>();
            var _jwtSecretKey = config["Jwt:Key"];

            var cilentId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (cilentId == null)
            {

               
                var list = (await userRepo.GetAll()).ToList();
                vm = list.Select(x => ((UserVm)x)).ToList();
                
            }
            else 
            {
               
                var list = (await userRepo.GetAll()).ToList();
                vm = list.Select(x => ((UserVm)x)).ToList();
               
            }
            return Ok(vm);
        }
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient(UserVm vm)
        {
            IGeneralResult<UserVm> result = new GeneralResult<UserVm>();
            var _jwtSecretKey = config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);


            if (userRepo.Any(x => x.Email == vm.Email && x.Name == vm.Name))
            {
                result.Message = "Email Already Registered!";
                result.Message = "Name Already Registered!";
            }
            else
            {
                TblUser abc = new TblUser
                {
                    Name = vm.Name,
                    UserName = vm.UserName,
                    Password = vm.Password,
                    Email = vm.Email,
                    Mobile = vm.Mobile,
                    //SubscriptionPlan = vm.SubscriptionPlan,
                   // CraetedBy = userId == null ? null : Convert.ToInt16(userId),
                   // ModifiedBy = userId == null ? null : Convert.ToInt16(userId),
                   // CreatedDate = System.DateTime.Now,
                    //ModofiedDate = System.DateTime.Now,
                    IsActive = true,
                   // IsDeleted = false

                };
            var client = await userRepo.Add(abc);
            if (client.Id != 0)
            {
                result.IsSuccess = true;
                result.Message = "Client Added Successfully";
            }
            else
            {
                result.Message = "already record with this name exist";
                result.Message = "something Went Wrong";
            }


        }

            return Ok(result);

    }


    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(int Id, ClientVM vm)
    {
        IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
        if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.Name == vm.Name))
        {
            result.Message = "Email Already Registered!";
            result.Message = "Name Already Registered!";
        }
        else
        {
            var res = await _clientRepo.GetById(Id);
            if (res != null)
            {
                if (Id != null || vm.Id == null)
                {
                    res.Id = Id;
                    res.Name = vm.Name;
                    res.Email = vm.Email;
                    res.Phone = vm.Phone;
                    res.SubscriptionPlan = vm.SubscriptionPlan;
                    res.Geolocation = vm.Geolocation;
                    res.IsActive = true;
                    //res.IsDelete = false;
                    await _clientRepo.Update(res);
                    result.Message = "Client data is updated successfully";
                    result.IsSuccess = true;
                    result.Data = (ClientVM)res;
                    return Ok(result);
                }
                else
                {
                    result.Message = "Id not found.";
                }
            }
            else
            {
                result.Message = "Id not found.";
            }
        }

        return Ok(result);
    }


}
}

