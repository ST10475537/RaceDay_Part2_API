
namespace RaceDay.API.Models
{
    public class EventRoute
    {
        public int RouteID { get; set; }

        public int EventID { get; set; }

        public string RouteName { get; set; } = string.Empty;

        public decimal DistanceKM { get; set; }

        public string? RouteDescription { get; set; }

        public string? RouteMapURL { get; set; }
    }
}
