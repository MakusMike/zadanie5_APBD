using Microsoft.AspNetCore.Mvc;

using zadanie5_APBD.Services;

namespace zadanie5_APBD.Controllers
{
    [ApiController]
    [Route("api/Clients")]

    public class ClientsController : Controller
    {
        private static IDbService _IdbService
;
        public ClientsController(IDbService idbSErvice)
        {
            _IdbService = idbSErvice;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int idClient)
        {
            if(_IdbService.DeleteClient(idClient).Result == false)
            {
                return BadRequest("Client already have trips");
            }
            return Ok("Client was deleted");
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            return Ok(await _IdbService.GetTrips());
        }
    }
}
