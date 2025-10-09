using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;

namespace Attendo.Application.Students.Commands
{
    public class UpdateStudentCommand : IRequest<StudentDto>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
