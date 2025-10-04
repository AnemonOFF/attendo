using MediatR;

namespace Atendo.Application.Students.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
