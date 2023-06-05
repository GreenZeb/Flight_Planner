using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using FlightPlaner.Core.Models;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public Flight GetFullFlight(int id)
        {
            return _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);
        }

        public bool FlightExists(Flight flight)
        {
            return _context.Flights.Any(f =>
        f.From.City.ToLower() == flight.From.City.ToLower() &&
        f.To.City.ToLower() == flight.To.City.ToLower() &&
        f.Carrier.ToLower() == flight.Carrier.ToLower() &&
        f.DepartureTime == flight.DepartureTime &&
        f.ArrivalTime == flight.ArrivalTime);
        }

        public SearchModel SearchFlights(SearchFlightsRequest search)
        {
            var items = _context.Flights
                    .Include(f => f.From)
                    .Include(f => f.To)
                    .AsEnumerable()
                    .Where(f => f.From.AirportCode == search.From &&
                                f.To.AirportCode == search.To &&
                                Convert.ToDateTime(f.DepartureTime) >= Convert.ToDateTime(search.DepartureDate)).ToList();

            return new SearchModel() { Page = 0, TotalItems = items.Count, Items = items };
        }
    }
}