using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Events.Queries
{
    public class GetEventsQuery : IRequest<IReadOnlyList<EventDto>> { }
}
