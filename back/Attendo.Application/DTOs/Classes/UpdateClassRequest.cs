namespace Attendo.Application.DTOs.Classes;

public class UpdateClassRequest
{
    public string? Name { get; set; }

    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }

    public string? Frequency { get; set; }

    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    public int? GroupId { get; set; }
}
