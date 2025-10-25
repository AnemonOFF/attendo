using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Commands.SetAttendance
{
    public class SetClassAttendanceCommand : IRequest<ClassResponse>
    {
        public int ClassId { get; set; }
        public List<int> StudentIds { get; set; } = new();
    }
}
