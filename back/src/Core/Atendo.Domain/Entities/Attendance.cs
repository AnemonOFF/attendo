namespace Atendo.Domain.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public AttendanceStatus Status { get; set; }
    }
}
