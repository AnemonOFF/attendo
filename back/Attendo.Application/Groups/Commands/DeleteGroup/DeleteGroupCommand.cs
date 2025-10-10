using MediatR;

namespace Attendo.Application.Groups.Commands
{
    public class DeleteGroupCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
