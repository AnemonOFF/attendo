using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;
using MediatR;

namespace Attendo.Application.Students.Commands
{
    public class CreateStudentCommand : IRequest<StudentDto>
    {
        public string FullName { get; set; } = string.Empty;
    }
}
