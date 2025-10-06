using Attendo.Domain.Entities;

namespace Attendo.Application.DTOs.Attendances
{
    public class AttendanceDto
    {
        public int Id { get; set; }           
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
