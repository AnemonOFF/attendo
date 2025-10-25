using Attendo.Application.DTOs;
using MediatR;

namespace Attendo.Application.Groups.Queries
{
    using Attendo.Application.DTOs;
    using Attendo.Application.DTOs.Groups;
    using MediatR;

    public class GetGroupByIdQuery : IRequest<GroupDto?>
    {
        public int Id { get; set; }
    }
}
