using MediatR;

namespace Atendo.Application.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
