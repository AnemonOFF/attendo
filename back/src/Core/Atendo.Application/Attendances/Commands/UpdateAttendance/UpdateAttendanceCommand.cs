using MediatR;
using Atendo.Application.DTOs;
using Atendo.Domain.Entities;

namespace Atendo.Application.Attendances.Commands.UpdateAttendance
{
    public class UpdateAttendanceCommand : IRequest<AttendanceDto>
    {
        public int Id { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
