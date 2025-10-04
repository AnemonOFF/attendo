using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Events.Queries
{
    public class GetEventByIdQuery : IRequest<EventDto?>
    {
        public int Id { get; set; }
    }
}
