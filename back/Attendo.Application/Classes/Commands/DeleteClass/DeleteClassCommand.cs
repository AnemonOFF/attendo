using MediatR;

namespace Attendo.Application.Classes.Commands.DeleteClass
{
    public class DeleteClassCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
