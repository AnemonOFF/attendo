using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Groups.Queries
{
    using MediatR;
    using Attendo.Application.DTOs;

    public class GetGroupByIdQuery : IRequest<GroupDto?>
    {
        public int Id { get; set; }
    }
}
