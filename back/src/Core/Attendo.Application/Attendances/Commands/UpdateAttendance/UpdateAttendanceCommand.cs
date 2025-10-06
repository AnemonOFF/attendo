using MediatR;
using Attendo.Application.DTOs.Attendances;
using Attendo.Domain.Entities;

namespace Attendo.Application.Attendances.Commands.UpdateAttendance
{
    public class UpdateAttendanceCommand : IRequest<AttendanceDto>
    {
        public int Id { get; set; }
        public AttendanceStatus Status { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
    }
}
