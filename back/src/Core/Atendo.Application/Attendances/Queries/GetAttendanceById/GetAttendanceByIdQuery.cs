using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Attendances.Queries.GetAttendanceById
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceDto?>
    {
        public int Id { get; set; }
    }
}
