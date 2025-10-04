using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Students.Queries
{
    public class GetStudentsQuery : IRequest<IReadOnlyList<StudentDto>> { }
}
