using MediatR;

namespace Atendo.Application.Groups.Commands
{
    public class DeleteGroupCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
