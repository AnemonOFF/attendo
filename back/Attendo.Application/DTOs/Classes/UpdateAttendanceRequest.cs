namespace Attendo.Application.DTOs.Classes
{
    public class UpdateAttendanceRequest
    {
        public List<int> StudentIds { get; set; } = new();
    }
}
