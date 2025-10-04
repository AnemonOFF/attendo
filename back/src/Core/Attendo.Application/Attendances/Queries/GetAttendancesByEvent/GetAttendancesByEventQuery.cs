using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Attendances.Queries.GetAttendancesByEvent
{
    public class GetAttendancesByEventQuery : IRequest<IReadOnlyList<AttendanceDto>>
    {
        public int EventId { get; set; }
    }
}
