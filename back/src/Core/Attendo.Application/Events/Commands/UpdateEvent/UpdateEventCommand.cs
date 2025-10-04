using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest<EventDto>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
