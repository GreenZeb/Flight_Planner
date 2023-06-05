using Microsoft.EntityFrameworkCore;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Services.Validators;

    public static class FlightValidation
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

                if (SameAirportValidation.IsSameAirport(flight.From, flight.To))
                    return false;

                var arrivalTime = DateTime.Parse(flight.ArrivalTime);
                var departureTime = DateTime.Parse(flight.DepartureTime);

                if (arrivalTime <= departureTime)
                    return false;

                return true;
            }
        }
    }