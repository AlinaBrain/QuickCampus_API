using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;

namespace QuickCampusAPI.Controllers
{
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
        [Route("GetClients")]
        public async Task<ActionResult> GetClients(int Id)
        {
            var model = await _clientRepo.GetAllClient();
            return Ok(model);
        }

        //[HttpGet]
        //[Route("Add")]
        //public async Task<ActionResult> Add()
        //{
        //    var model = new ClientVM()
        //    {
        //        rec = (await _clientRepo.GetAllClient()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }),

        //    };
        //    return Ok(model);
        //}

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ClientVM clientVM)
        {
            IGeneralResult<ClientVM> result = new GeneralResult<ClientVM>();
            var client = await _clientRepo.Add(clientVM);
            if (client.Id != 0)
            {
                result.IsSuccess = true;
                result.Message = "Client Added Successfully";
            }
            else
            {
                result.Message = "something Went Wrong";
            }
            return Ok(result);
        }

    }
}
