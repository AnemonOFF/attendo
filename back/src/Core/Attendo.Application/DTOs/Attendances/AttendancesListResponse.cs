namespace Attendo.Application.DTOs.Attendances
{
    public class AttendancesListResponse
    {
        public IList<AttendanceDto> Items { get; set; } = new List<AttendanceDto>();
    }
}
