namespace Attendo.Domain.Entities;

public class ClassAttendanceStudent
{
    public int ClassAttendanceId { get; set; }
    public ClassAttendance ClassAttendance { get; set; } = null!;

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}
