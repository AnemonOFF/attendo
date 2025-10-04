using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Attendances.Queries.GetAttendancesByEvent
{
    public class GetAttendancesByEventQuery : IRequest<IReadOnlyList<AttendanceDto>>
    {
        public int EventId { get; set; }
    }
}
