using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Students.Commands
{
    public class CreateStudentCommand : IRequest<StudentDto>
    {
        public string FullName { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
