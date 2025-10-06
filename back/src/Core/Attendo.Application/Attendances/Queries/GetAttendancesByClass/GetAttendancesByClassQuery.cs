using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Attendances;

namespace Attendo.Application.Attendances.Queries.GetAttendancesByClass
{
    public class GetAttendancesByClassQuery : IRequest<IReadOnlyList<AttendanceDto>>
    {
        public int ClassId { get; set; }
    }
}
