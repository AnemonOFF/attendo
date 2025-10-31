namespace Attendo.Domain.Entities;

public class Class
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public string Frequency { get; set; } = default!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public ICollection<ClassAttendance> Attendance { get; set; } = new List<ClassAttendance>();
}
