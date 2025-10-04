using MediatR;
using Attendo.Application.DTOs;
using Attendo.Domain.Entities;

namespace Attendo.Application.Attendances.Commands.CreateAttendance
{
    public class CreateAttendanceCommand : IRequest<AttendanceDto>
    {
        public int StudentId { get; set; }
        public int EventId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
