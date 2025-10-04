using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
