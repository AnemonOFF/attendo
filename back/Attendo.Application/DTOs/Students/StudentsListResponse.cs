namespace Attendo.Application.DTOs.Students
{
    public class StudentsListResponse
    {
        public IList<StudentDto> Items { get; set; } = new List<StudentDto>();
    }
}
