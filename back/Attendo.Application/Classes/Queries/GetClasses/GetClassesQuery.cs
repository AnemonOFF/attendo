using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Queries
{
    public class GetClassesQuery : IRequest<ClassesListResponse> { }
}
