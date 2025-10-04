using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Attendances.Queries.GetAttendances
{
    public class GetAttendancesQuery : IRequest<IReadOnlyList<AttendanceDto>> { }
}
