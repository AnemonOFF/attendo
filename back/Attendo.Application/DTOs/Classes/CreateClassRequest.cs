namespace Attendo.Application.DTOs.Classes;

public class CreateClassRequest
{
    public string Name { get; set; } = string.Empty;

    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public string Frequency { get; set; } = string.Empty;

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public int GroupId { get; set; }
}
