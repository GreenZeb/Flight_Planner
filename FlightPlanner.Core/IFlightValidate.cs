using FlightPlanner.Core.Models;

namespace FlightPlanner.Core
{
    public interface IFlightValidate
    {
        bool IsValid(Flight flight);
    }
}