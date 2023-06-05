using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Core.Services;
using FlightPlanner.Core.Models;

namespace FlightPlanner_ASPNET.Controllers;

[ApiController]
[Route("testing-api")]
public class CleanupController : ControllerBase
{
    private readonly IDbService _dbService;

    public CleanupController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    [Route("clear")]
    public IActionResult Clear()
    {
        _dbService.DeleteAll<Flight>();
        _dbService.DeleteAll<Airport>();

        return Ok();
    }
}