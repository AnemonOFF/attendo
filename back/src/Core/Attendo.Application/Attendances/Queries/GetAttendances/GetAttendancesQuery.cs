using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Attendances.Queries.GetAttendances
{
    public class GetAttendancesQuery : IRequest<IReadOnlyList<AttendanceDto>> { }
}
