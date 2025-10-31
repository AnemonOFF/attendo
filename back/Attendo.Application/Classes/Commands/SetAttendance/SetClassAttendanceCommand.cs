using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Commands.SetAttendance;

public sealed class SetClassAttendanceCommand(int classId, List<AttendanceItem> attendance) : IRequest<ClassResponse>
{
    public int ClassId { get; } = classId;
    public List<AttendanceItem> Attendance { get; } = attendance;
}
