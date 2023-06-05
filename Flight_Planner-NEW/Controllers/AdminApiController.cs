using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Core;
using FlightPlanner.Services.Validators;
using FlightPlanner_ASPNET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner_ASPNET.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IFlightValidate> _validators;

        public AdminApiController (
                IFlightService flightService,
                IMapper mapper,
                IEnumerable<IFlightValidate> validators)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validators = validators;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = _flightService.GetFullFlight(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AddFlightRequest>(flight));
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest request)
        {
            var flight = _mapper.Map<Flight>(request);

            lock (_lock)
            {
                if (_flightService.FlightExists(flight))
                {
                    return Conflict();
                }

                if (!FlightValidation.IsValid(flight))
                {
                    return BadRequest();
                }

                _flightService.Create(flight);

                return Created("", _mapper.Map<AddFlightRequest>(flight));
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlights(int id)
        {
            var flight = _flightService.GetFullFlight(id);
            lock (_lock)
            {
                if (flight != null)
                {
                    _flightService.Delete(flight);
                    return Ok();
                }
                else
                {
                    return Ok();
                }
            }
        }
    }
}