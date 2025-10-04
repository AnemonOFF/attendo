using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Students.Commands
{
    public class UpdateStudentCommand : IRequest<StudentDto>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
