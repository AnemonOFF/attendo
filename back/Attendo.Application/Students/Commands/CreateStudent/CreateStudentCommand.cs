using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;

namespace Attendo.Application.Students.Commands
{
    public class CreateStudentCommand : IRequest<StudentDto>
    {
        public string FullName { get; set; } = string.Empty;
    }
}
