namespace FlightPlaner_ASPNET.Models
{
    public class SearchModel
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public List<Flight> Items { get; set; } = new List<Flight>();
    }
}