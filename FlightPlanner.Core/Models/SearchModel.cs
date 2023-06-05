
namespace FlightPlanner.Core.Models;
    public class SearchModel
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public Array Items { get; set; }

        public SearchModel(int totalItems, Array items)
        {
            Page = 0;
            TotalItems = totalItems;
            Items = items;
        }
    }