namespace Atendo.Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public List<Attendance> Attendances { get; set; } = new();
}
