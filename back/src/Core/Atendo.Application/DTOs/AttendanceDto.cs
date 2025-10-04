using Atendo.Domain.Entities;

namespace Atendo.Application.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int EventId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
