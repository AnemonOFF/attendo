using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Events.Queries
{
    public class GetEventsQuery : IRequest<IReadOnlyList<EventDto>> { }
}
