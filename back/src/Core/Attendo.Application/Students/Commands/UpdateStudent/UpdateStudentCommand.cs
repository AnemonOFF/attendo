using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Students.Commands
{
    public class UpdateStudentCommand : IRequest<StudentDto>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
