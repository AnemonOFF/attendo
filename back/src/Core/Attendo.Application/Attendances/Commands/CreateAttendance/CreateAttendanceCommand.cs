using MediatR;
using Attendo.Application.DTOs.Attendances;
using Attendo.Domain.Entities;

namespace Attendo.Application.Attendances.Commands.CreateAttendance
{
    public class CreateAttendanceCommand : IRequest<AttendanceDto>
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
