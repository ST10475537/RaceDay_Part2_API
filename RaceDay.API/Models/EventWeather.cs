
namespace RaceDay.API.Models
{
    public class EventWeather
    {
        public int WeatherID { get; set; }

        public int EventID { get; set; }

        public DateTime ForecastDate { get; set; }

        public decimal? Temperature { get; set; }

        public string? WeatherCondition { get; set; }

        public decimal? WindSpeed { get; set; }

        public DateTime RetrievedAt { get; set; }
    }
}
