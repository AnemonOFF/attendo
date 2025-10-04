using Attendo.Domain.Entities;

namespace Attendo.Application.DTOs
{
    public class AttendanceCreateDto
    {
        public int StudentId { get; set; }
        public int EventId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
