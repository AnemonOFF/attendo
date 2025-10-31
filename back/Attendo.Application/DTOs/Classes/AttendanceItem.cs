namespace Attendo.Application.DTOs.Classes;
public class AttendanceItem
{
    public DateOnly Date { get; set; }
    public List<int> Students { get; set; } = new();
}
