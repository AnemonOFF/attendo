using Attendo.Application.DTOs.Attendances;

namespace Attendo.Application.DTOs.Classes;

public class ClassResponse
{
    public int Id { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public int GroupId { get; set; }

    public IList<AttendanceDto> Attendance { get; set; } = new List<AttendanceDto>();
}
