using Microsoft.EntityFrameworkCore;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Services.Validators;

    public static class SearchValidation
    {
        public static bool IsValidSearch(SearchFlightsRequest request)
    {
        if (request == null)
            return false;

        if (string.IsNullOrEmpty(request.DepartureDate) ||
            string.IsNullOrEmpty(request.From) ||
            string.IsNullOrEmpty(request.To))
            return false;

        if (request.From == request.To)
            return false;

        return true;
    }
}