using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    

    public class ClientController : ControllerBase
    {
     private readonly IClientRepo _clientRepo;
        private IConfiguration config;
        public ClientController(IClientRepo clientRepo, IConfiguration config)
        {
            _clientRepo = clientRepo;
            this.config = config;
        }

        [HttpGet]
        [Route("ClientList")]
        public async Task<IActionResult> ClientList()
        {
            IGeneralResult<List<ClientVM>> result = new GeneralResult<List<ClientVM>>();
            try
            {
                var clientList = (await _clientRepo.GetAll()).ToList();
                var res = clientList.Select(x => ((ClientVM)x)).ToList();
                if (res != null)
                {
                    result.IsSuccess = true;
                    result.Message = "ClientList";
                    result.Data = res;
                }
                else
                {
                    result.Message = "Client List Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Server Error";
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient(ClientVM vm)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var _jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);

            if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.Name == vm.Name))
            {
                result.Message = "Email Already Registered!";
                result.Message = "Name Already Registered!";
            }
            else
            {
                var client = await _clientRepo.Add(vm.ToClientDbModel());
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

