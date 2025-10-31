namespace Attendo.Domain.Entities;

public class ClassAttendance
{
    public int Id { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    public DateOnly Date { get; set; }

    public ICollection<ClassAttendanceStudent> Students { get; set; } = new List<ClassAttendanceStudent>();
}
