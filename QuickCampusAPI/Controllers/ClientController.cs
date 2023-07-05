using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    

    public class ClientController : ControllerBase
    {
     private readonly IClientRepo _clientRepo;
        public ClientController(IClientRepo clientRepo)
        {
            _clientRepo = clientRepo;
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
        public async Task<IActionResult> AddClient(ClientVM clientVM)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();

            if (ModelState.IsValid)
            {
                if (!_clientRepo.Any(o => o.Name == clientVM.Name))
                {
                    var client = await _clientRepo.Add(clientVM.ToClientDbModel());
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
            }

            return Ok(result);
                
                
            
        }
        [HttpPatch]
        [Route("EditClient")]
        public async Task<IActionResult> EditClient(ClientVM clientVM)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var clientDetail = await _clientRepo.GetById(clientVM.Id);
            if (clientDetail != null)
            {
                clientDetail.Name = clientVM.Name;
                clientDetail.Phone = clientVM.Phone;
                clientDetail.Email = clientVM.Email;
                clientDetail.SubscriptionPlan = clientVM.SubscriptionPlan;
                clientDetail.Geolocation = clientVM.Geolocation;
                //clientDetail.ModifiedBy = clientVM.ModifiedBy;
                clientDetail.ModofiedDate = clientVM.ModofiedDate;
                //clientDetail.CraetedBy = clientVM.CraetedBy;
                clientDetail.CreatedDate = clientVM.CreatedDate;
                clientDetail.Address = clientVM.Address;

                await _clientRepo.Update(clientDetail);
               
                result.Message = "Client Details updated Succesfully";
                result.IsSuccess = true;
            }
            else
            {
                result.Message = "Client details does not found";
            }
            return Ok(result);
        }
    }
}

