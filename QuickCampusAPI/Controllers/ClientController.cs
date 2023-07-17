using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

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

        [Authorize(Roles = "AddClient")]
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient([FromBody] ClientVM vm)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var _jwtSecretKey = config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true))
            {
                result.Message = "Email Already Registered!";
            }
            //else if (_clientRepo.Any(x => x.IsActive == true && x.Name == vm.Name.Trim()))
            //{
            //    result.Message = "UserName Already Exist!";
            //}
            else
            {
                if (ModelState.IsValid)
                {

                    ClientVM vmm = new ClientVM
                    {
                        Name = vm.Name,
                        Email = vm.Email,
                        Phone = vm.Phone,
                        Address = vm.Address,
                        CreatedDate = vm.CreatedDate,
                        ModofiedDate = vm.ModofiedDate,
                        IsDeleted = vm.IsDeleted,   
                        SubscriptionPlan = vm.SubscriptionPlan,
                        CraetedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        Latitude=vm.Latitude,
                        Longitude=vm.Longitude,
                        IsActive=vm.IsActive,
                        
                    };
                    try
                    {
                        result.Data = vmm;
                        var TblClien = vmm.ToClientDbModel();
                        await _clientRepo.Add(TblClien);
                        result.Message = "Client added successfully";
                        result.IsSuccess = true;
                       
                    }
                    catch(Exception ex)
                    {
                        result.Message = ex.Message;
                    }
                  
                    return Ok(result);
                }
                    else
                    {
                        result.Message = "something Went Wrong";
                    }

            }
            return Ok(result);
        }

        [Authorize(Roles = "EditClient")]
        [HttpPost]
        [Route("EditClient")]
        public async Task<IActionResult> EditClient([FromBody] ClientVM vm)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var _jwtSecretKey = config["Jwt:Key"];
            var userId = JwtHelper.GetIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            if (_clientRepo.Any(x => x.Email == vm.Email && x.IsActive == true && x.Id != vm.Id))
            {
                result.Message = "Email Already Registered!";
            }
            else
            {
                if (ModelState.IsValid && vm.Id>0)
                {

                    ClientVM vmm = new ClientVM
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Email = vm.Email,
                        Phone = vm.Phone,
                        Address = vm.Address,
                        IsDeleted = vm.IsDeleted,
                        IsActive = vm.IsActive,
                        SubscriptionPlan = vm.SubscriptionPlan,
                        CraetedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        Longitude = vm.Longitude,
                        Latitude=vm.Latitude
                    };
                    try
                    {
                        var TblClien = vmm.ToUpdateDbModel();
                        await _clientRepo.Update(TblClien);
                        result.Message = "Client updated successfully";
                        result.IsSuccess = true;
                        result.Data = vmm;
                    }
                    catch (Exception ex)
                    {
                        result.Message=ex.Message;
                    }        
                    return Ok(result);
                }
                else
                {
                    result.Message = "something Went Wrong";
                }

            }
            return Ok(result);
        }

        [Authorize(Roles = "GetAllClient")]
        [HttpGet]
        [Route("GetAllClient")]
        public async Task<IActionResult> GetAllClient()
        {
            var _jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<List<ClientVM>> result = new GeneralResult<List<ClientVM>>();
            try
            {
                var categoryList = (await _clientRepo.GetAll()).Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
                var res = categoryList.Select(x => ((ClientVM)x)).ToList();
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
                result.Message = ex.Message;
            }
            return Ok(result);
        }

        [Authorize(Roles = "DeleteClient")]
        [HttpDelete]
        [Route("DeleteClient")]
        public async Task<IActionResult> DeleteClient(int Id)
        {
            var _jwtSecretKey = config["Jwt:Key"];
            var clientId = JwtHelper.GetClientIdFromToken(Request.Headers["Authorization"], _jwtSecretKey);
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var res = await _clientRepo.GetById(Id);
            if (res != null)
            {
              
                res.IsActive = false;
                res.IsDeleted = true;
                await _clientRepo.Update(res);
                result.IsSuccess = true;
                result.Message = "Client Deleted Succesfully";
            }
            else
            {
                result.Message = "Client does Not exist";
            }
            return Ok(result);
        }

    }
}

