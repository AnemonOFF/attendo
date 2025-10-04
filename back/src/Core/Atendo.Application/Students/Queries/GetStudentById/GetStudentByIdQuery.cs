using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDto?>
    {
        public int Id { get; set; }
    }
}
