using FlightPlaner_ASPNET.Models;
using FlightPlaner_ASPNET.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner_ASPNET.Controllers;

[Route("admin-api")]
[ApiController]
[Authorize]
public class AdminApiController : ControllerBase
{
    private static readonly object _lock = new object();

    [HttpGet]
    [Route("flights/{id}")]
    public IActionResult GetFlights(int id)
    {
        var flight = FlightStorage.GetFlight(id);
        if (flight == null)
        {
            return NotFound();
        }

        return Ok(flight);
    }

    [HttpPut]
    [Route("flights")]
    public IActionResult AddFlight(Flight flight)
    {
        lock (_lock)
        {
            if (FlightStorage.Exists(flight))
                return Conflict();

            if (!FlightStorage.IsValid(flight))
                return BadRequest();

            if (FlightStorage.IsItTheSameAirport(flight))
                return BadRequest();

            return Created("", FlightStorage.AddFlight(flight));
        }
    }

    [HttpDelete]
    [Route("flights/{id}")]
    public IActionResult DeleteFlights(int id)
    {
        {
            FlightStorage.DeleteFlight(id);
            return Ok();
        }
    }
}
