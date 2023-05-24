using FlightPlaner_ASPNET.Models;
using FlightPlaner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlaner_ASPNET.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : BaseApiController
    {
        private static readonly object _lock = new object();

        public CustomerApiController(FlightPlannerDbContext context) : base(context)
        {
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports([FromQuery] string search)
        {
            lock (_lock)
            {
                search = search.ToLower().Trim();
                var airports = _context.Airports.Where(a =>
                        a.AirportCode.ToLower().Trim().Contains(search) ||
                        a.Country.ToLower().Trim().Contains(search) ||
                        a.City.ToLower().Trim().Contains(search))
                    .ToList();
                return Ok(airports);
            }
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest request)
        {
            if (!Validation.IsValidSearch(request))
            {
                return BadRequest();
            }

            var flights = new List<Flight>();

            lock (_lock)
            {
                foreach (var flight in _context.Flights.Include(f => f.To).Include(f => f.From))
                {
                    if (request.To == flight.To.AirportCode &&
                        request.From == flight.From.AirportCode &&
                        request.DepartureDate == flight.DepartureTime.Substring(0, 10))
                    {
                        flights.Add(flight);
                    }
                }
            }
            return Ok(new SearchModel(flights.Count, flights.ToArray()));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = _context.Flights.Include(f => f.From).Include(f => f.To).SingleOrDefault(f => f.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

    }
}