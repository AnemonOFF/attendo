using Attendo.Application.DTOs.Groups;

namespace Attendo.Application.DTOs.Classes;

public class ClassResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public string Frequency { get; set; } = string.Empty;

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public GroupResponse Group { get; set; } = null!;
}
