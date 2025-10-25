using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Queries
{
    public class GetClassByIdQuery : IRequest<ClassDto?>
    {
        public int Id { get; set; }
    }
}
