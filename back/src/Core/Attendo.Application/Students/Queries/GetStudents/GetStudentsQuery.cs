using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;

namespace Attendo.Application.Students.Queries
{
    public class GetStudentsQuery : IRequest<IReadOnlyList<StudentDto>> { }
}
