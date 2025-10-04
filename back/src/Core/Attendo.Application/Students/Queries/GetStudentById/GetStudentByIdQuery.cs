using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDto?>
    {
        public int Id { get; set; }
    }
}
