using FlightPlaner_ASPNET.Models;
using FlightPlaner_ASPNET.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlaner_ASPNET.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports([FromQuery]string search)
        {
            var airport = FlightStorage.FindAirports(search);

            return Ok(airport);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest request)
        {
            if (!FlightStorage.IsValidSearch(request))
                return BadRequest();
            return Ok(FlightStorage.SearchFlight(request));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
                return NotFound();
            return Ok(flight);
        }
    }
}
