using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Students.Commands
{
    public class CreateStudentCommand : IRequest<StudentDto>
    {
        public string FullName { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
