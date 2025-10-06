using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Attendances.Queries.GetAttendancesByClass
{
    public class GetAttendancesByClassQuery : IRequest<IReadOnlyList<AttendanceDto>>
    {
        public int ClassId { get; set; }
    }
}
