using System.Collections.Generic;
using System.Linq;
using FlightPlaner_ASPNET.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlaner_ASPNET.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;
        private static object _lock = new object();

        public static Flight GetFlight(int id)
        {
            lock (_lock)
            {
                return _flights.SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static Flight AddFlight(Flight flight)
        {
            lock (_lock)
            {
                flight.Id = _id++;
                _flights.Add(flight);
                return flight;
            }
        }

        public static void DeleteFlight(int id)
        {
            lock (_lock)
            {
                var flight = GetFlight(id);
                if (flight != null)
                    _flights.Remove(flight);
            }
        }

        public static List<Airport> FindAirports(string userInput)
        {
            lock (_lock)
            {
                userInput = userInput.ToLower().Trim();
                var fromAirport = _flights.Where(a =>
                        a.From.AirportCode.ToLower().Trim().Contains(userInput) ||
                        a.From.Country.ToLower().Trim().Contains(userInput) ||
                        a.From.City.ToLower().Trim().Contains(userInput))
                    .Select(a => a.From).ToList();
                var toAirport = _flights.Where(a =>
                    a.To.AirportCode.ToLower().Trim().Contains(userInput) ||
                    a.To.Country.ToLower().Trim().Contains(userInput) ||
                    a.To.City.ToLower().Trim().Contains(userInput)).Select(a => a.To).ToList();
                return fromAirport.Concat(toAirport).ToList();
            }
        }

        public static void Clear()
        {
            _flights.Clear();
        }
        public static bool Exists(Flight flight)
        {
            lock (_lock)
            {
                foreach (var f in _flights)
                {
                    if ((flight.ArrivalTime).ToLower().Trim() == (f.ArrivalTime).ToLower().Trim() &&
                        (flight.Carrier).ToLower().Trim() == (f.Carrier).ToLower().Trim() &&
                        (flight.DepartureTime).ToLower().Trim() == (f.DepartureTime).ToLower().Trim() &&
                        (flight.From.AirportCode).ToLower().Trim() == (f.From.AirportCode).ToLower().Trim() &&
                        (flight.From.City).ToLower().Trim() == (f.From.City).ToLower().Trim() &&
                        (flight.From.Country).ToLower().Trim() == (f.From.Country).ToLower().Trim() &&
                        (flight.To.AirportCode).ToLower().Trim() == (f.To.AirportCode).ToLower().Trim() &&
                        (flight.To.City).ToLower().Trim() == (f.To.City).ToLower().Trim() &&
                        (flight.To.Country).ToLower().Trim() == (f.To.Country).ToLower().Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
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

                if (flight.From.Country.ToLower().Trim() == flight.To.Country.ToLower().Trim() &&
                    flight.From.City.ToLower().Trim() == flight.To.City.ToLower().Trim() &&
                    flight.From.AirportCode.ToLower().Trim() == flight.To.AirportCode.ToLower().Trim())
                    return false;

                var arrivalTime = DateTime.Parse(flight.ArrivalTime);
                var departureTime = DateTime.Parse(flight.DepartureTime);

                if (arrivalTime <= departureTime)
                    return false;

                return true;
            }
        }

        public static SearchModel SearchFlight(SearchFlightsRequest request)
        {
            var resultList = new SearchModel() { Page = 0, TotalItems = 0 };
            foreach (var flight in _flights)
            {
                if (request.From == flight.From.AirportCode &&
                    request.To == flight.To.AirportCode &&
                    Convert.ToDateTime(request.DepartureDate) < Convert.ToDateTime(flight.DepartureTime))
                {
                    resultList.TotalItems++;
                    resultList.Items.Add(flight);
                }
            }
            return resultList;
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
            public static bool IsItTheSameAirport(Flight flight)
            {
            lock (_lock)
            {
                return flight.From.AirportCode.ToLower().Trim() == flight.To.AirportCode.ToLower().Trim();
            }
        }
    }
}
