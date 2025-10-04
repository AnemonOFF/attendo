using MediatR;

namespace Atendo.Application.Attendances.Commands.DeleteAttendance
{
    public class DeleteAttendanceCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
