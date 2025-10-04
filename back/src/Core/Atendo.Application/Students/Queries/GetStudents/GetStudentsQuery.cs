using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Students.Queries
{
    public class GetStudentsQuery : IRequest<IReadOnlyList<StudentDto>> { }
}
