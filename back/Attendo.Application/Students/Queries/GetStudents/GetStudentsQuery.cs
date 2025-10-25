using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;
using MediatR;

namespace Attendo.Application.Students.Queries
{
    public class GetStudentsQuery : IRequest<IReadOnlyList<StudentDto>> { }
}
