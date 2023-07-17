using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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
                        Geolocation = vm.Geolocation,
                        SubscriptionPlan = vm.SubscriptionPlan,
                        CraetedBy = Convert.ToInt32(userId),
                        ModifiedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        ModofiedDate=DateTime.Now
                    };
                    try
                    {
                        var TblClien = vmm.ToClientDbModel();
                        await _clientRepo.Add(TblClien);
                       // await _clientRepo.Save();
                    }
                    catch(Exception ex)
                    {

                    }
                    result.Message = "Client added successfully";
                    result.IsSuccess = true;
                    result.Data = vmm;
                    return Ok(result);
                }
                    else
                    {
                        result.Message = "something Went Wrong";



                    }

            }
            return Ok(result);
        }

      

    }
}

