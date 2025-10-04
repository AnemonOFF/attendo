using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest<EventDto>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
