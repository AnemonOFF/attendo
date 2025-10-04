using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Groups.Queries
{
    using MediatR;
    using Atendo.Application.DTOs;

    public class GetGroupByIdQuery : IRequest<GroupDto?>
    {
        public int Id { get; set; }
    }
}
