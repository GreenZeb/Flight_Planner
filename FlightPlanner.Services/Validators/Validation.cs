using Microsoft.EntityFrameworkCore;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Services.Validators;

    public static class Validation
    {
        private static object _lock = new object();
       

        public static bool IsValid(Flight flight)
        {
            lock (_lock)
            {
                if (flight == null)
                    return false;

                if (string.IsNullOrEmpty(flight.ArrivalTime) ||
                    string.IsNullOrEmpty(flight.Carrier) ||
                    string.IsNullOrEmpty(flight.DepartureTime))
                    return false;

                if (flight.From == null ||
                    flight.To == null)
                    return false;

                if (string.IsNullOrEmpty(flight.From.AirportCode) ||
                    string.IsNullOrEmpty(flight.From.City) ||
                    string.IsNullOrEmpty(flight.From.Country))
                    return false;

                if (string.IsNullOrEmpty(flight.To.AirportCode) ||
                    string.IsNullOrEmpty(flight.To.City) ||
                    string.IsNullOrEmpty(flight.To.Country))
                    return false;

                if (IsSameAirport(flight.From, flight.To))
                    return false;

                var arrivalTime = DateTime.Parse(flight.ArrivalTime);
                var departureTime = DateTime.Parse(flight.DepartureTime);

                if (arrivalTime <= departureTime)
                    return false;

                return true;
            }
        }

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

        public static bool IsSameAirport(Airport from, Airport to)
        {
            return from.AirportCode.ToLower().Trim() == to.AirportCode.ToLower().Trim();
        }
    }