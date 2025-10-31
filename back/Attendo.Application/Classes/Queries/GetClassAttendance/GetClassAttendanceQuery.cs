using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Queries.GetClassAttendance;

public sealed class GetClassAttendanceQuery : IRequest<ClassAttendanceResponse>
{
    public int ClassId { get; set; }
}
