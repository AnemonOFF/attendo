namespace Attendo.Application.DTOs.Attendances;

public class UpdateAttendanceRequest
{
    public IList<int> StudentIds { get; set; } = new List<int>();
}
