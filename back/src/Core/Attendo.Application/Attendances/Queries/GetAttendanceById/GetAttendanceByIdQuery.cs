using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Attendances;

namespace Attendo.Application.Attendances.Queries.GetAttendanceById
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceDto?>
    {
        public int Id { get; set; }
    }
}
