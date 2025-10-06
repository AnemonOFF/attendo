using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Classes.Queries
{
    public class GetClassByIdQuery : IRequest<ClassDto?>
    {
        public int Id { get; set; }
    }
}
