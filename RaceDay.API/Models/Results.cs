
namespace RaceDay.API.Models
{
    public class Result
    {
        public int ResultID { get; set; }

        public int EnrollmentID { get; set; }

        public int? FinishPosition { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public decimal? Pace { get; set; }

        public string ResultStatus { get; set; } = "Completed";
    }
}
