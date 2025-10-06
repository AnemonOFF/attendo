using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Classes.Queries
{
    public class GetClassesQuery : IRequest<IReadOnlyList<ClassDto>> { }
}
