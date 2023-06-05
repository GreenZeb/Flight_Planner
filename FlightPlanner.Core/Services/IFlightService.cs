using FlightPlaner.Core.Models;
using FlightPlanner.Core.Models;
using System.Data.Entity.Core.Common.EntitySql;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight GetFullFlight(int id);
        bool FlightExists(Flight flight);
        SearchModel SearchFlights(SearchFlightsRequest search);
    }
}