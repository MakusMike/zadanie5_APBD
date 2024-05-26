using Microsoft.AspNetCore.Mvc;
using zadanie5_APBD.Services;

namespace zadanie5_APBD.Controllers
{   
    public class ClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
    
    [ApiController]
    [Route("api/trip")]
    
    public class TripsController : Controller
    {

        private static IDbService _idbService;

        public TripsController(IDbService iDbService)
        {
            _idbService = iDbService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            return Ok(await _idbService.GetTrips());
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientDto clientDto)
        {
            try
            {
                await _idbService.AddClientToTrip(idTrip, clientDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    
}
