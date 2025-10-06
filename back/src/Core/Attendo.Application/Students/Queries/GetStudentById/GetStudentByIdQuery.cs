using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;

namespace Attendo.Application.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDto?>
    {
        public int Id { get; set; }
    }
}
