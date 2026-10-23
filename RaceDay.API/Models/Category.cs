
namespace RaceDay.API.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal DistanceKM { get; set; }

        public int? MaximumParticipants { get; set; }

        public decimal EntryFee { get; set; }
    }
}
