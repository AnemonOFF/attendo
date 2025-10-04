using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Attendances.Queries.GetAttendanceById
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceDto?>
    {
        public int Id { get; set; }
    }
}
