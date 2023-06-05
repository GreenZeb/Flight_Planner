using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Services.Validators;
using FlightPlanner_ASPNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner_ASPNET.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IDbService _dbService;
        private readonly IMapper _mapper;

        public CustomerApiController(IFlightService flightService, IDbService dbService, IMapper mapper)
        {
            _flightService = flightService;
            _dbService = dbService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports([FromQuery] string search)
        {
            {
                search = search.ToLower().Trim();
                var airports = _dbService.GetAll<Airport>().Where(a =>
                        a.AirportCode.ToLower().Trim().Contains(search) ||
                        a.Country.ToLower().Trim().Contains(search) ||
                        a.City.ToLower().Trim().Contains(search))
                    .ToList();
                var mappedAirports = _mapper.Map<AddAirportRequest[]>(airports);

                return Ok(mappedAirports);
            }
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest request)
        {
            if (!SearchValidation.IsValidSearch(request))
            {
                return BadRequest();
            }

            var flights = new List<Flight>();

                foreach (var flight in _flightService.GetAll<Flight>())
                {
                    if (flight.To != null && flight.From != null &&
                        request.To == flight.To?.AirportCode &&
                        request.From == flight.From?.AirportCode &&
                        request.DepartureDate == flight.DepartureTime.Substring(0, 10))
                    {
                        flights.Add(flight);
                    }
                }

            return Ok(_flightService.SearchFlights(request));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = _flightService.GetFullFlight(id);
            if (flight == null)
                return NotFound();

            return Ok(_mapper.Map<AddFlightRequest>(flight));
        }
    }
}