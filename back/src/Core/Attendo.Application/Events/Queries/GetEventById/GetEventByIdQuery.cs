using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Events.Queries
{
    public class GetEventByIdQuery : IRequest<EventDto?>
    {
        public int Id { get; set; }
    }
}
