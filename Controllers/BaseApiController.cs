using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner_ASPNET.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected FlightPlannerDbContext _context;
        public BaseApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }
    }
}