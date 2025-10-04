using MediatR;
using Atendo.Application.DTOs;
using Atendo.Domain.Entities;

namespace Atendo.Application.Attendances.Commands.CreateAttendance
{
    public class CreateAttendanceCommand : IRequest<AttendanceDto>
    {
        public int StudentId { get; set; }
        public int EventId { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
