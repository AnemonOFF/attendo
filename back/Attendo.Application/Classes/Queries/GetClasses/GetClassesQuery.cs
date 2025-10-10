using MediatR;
using Attendo.Application.DTOs.Classes;

namespace Attendo.Application.Classes.Queries
{
    public class GetClassesQuery : IRequest<ClassesListResponse> { }
}
