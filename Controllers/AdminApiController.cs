using FlightPlanner.Core.Models;
using FlightPlanner.Data;
using FlightPlanner.Services.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner_ASPNET.Controllers;

[Route("admin-api")]
[ApiController]
[Authorize]
public class AdminApiController : BaseApiController
{
    private static readonly object _lock = new object();

    public AdminApiController(FlightPlannerDbContext context) : base(context)
    {
    }

    [HttpGet]
    [Route("flights/{id}")]
    public IActionResult GetFlightById(int id)
    {
        var flight = _context.Flights.SingleOrDefault(f => f.Id == id);
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
            if (_context.Flights.Any(f => f.From.City.ToLower() == flight.From.City.ToLower()
                                             && f.To.City.ToLower() == flight.To.City.ToLower()
                                             && f.Carrier.ToLower() == flight.Carrier.ToLower()
                                             && f.DepartureTime == flight.DepartureTime &&
                                             f.ArrivalTime == flight.ArrivalTime))
            {
                return Conflict();
            }

            if (!Validation.IsValid(flight))
            {
                return BadRequest();
            }

            _context.Flights.Add(flight);
            _context.SaveChanges();

            return Created("", flight);
        }
    }

    [HttpDelete]
    [Route("flights/{id}")]
    public IActionResult DeleteFlights(int id)
    {
        lock (_lock)
        {
            var flight = _context.Flights.SingleOrDefault(f => f.Id == id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return Ok();
            }
        }
    }
}
