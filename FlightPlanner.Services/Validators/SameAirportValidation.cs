using Microsoft.EntityFrameworkCore;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Services.Validators;

    public static class SameAirportValidation
    {

    public static bool IsSameAirport(Airport from, Airport to)
    {
        return from.AirportCode.ToLower().Trim() == to.AirportCode.ToLower().Trim();
    }
}