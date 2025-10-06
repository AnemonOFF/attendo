using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Attendances;

namespace Attendo.Application.Attendances.Queries.GetAttendances
{
    public class GetAttendancesQuery : IRequest<IReadOnlyList<AttendanceDto>> { }
}
