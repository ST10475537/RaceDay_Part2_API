
namespace RaceDay.API.Models
{
    public class EventEnrollment
    {
        public int EnrollmentID { get; set; }

        public int EventID { get; set; }

        public int ParticipantID { get; set; }

        public int CategoryID { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public string Status { get; set; } = "Active";
    }
}
