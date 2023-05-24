using FlightPlaner_ASPNET.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlaner_ASPNET
{
    public class FlightPlannerDbContext : DbContext
    {
        public FlightPlannerDbContext(DbContextOptions<FlightPlannerDbContext> options) : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }

    }
}