namespace Attendo.Domain.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    }
}
