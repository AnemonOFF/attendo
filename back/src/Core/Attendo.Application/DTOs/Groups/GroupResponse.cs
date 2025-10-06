using Attendo.Application.DTOs.Students;

namespace Attendo.Application.DTOs.Groups;

public class GroupResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IList<StudentDto> Students { get; set; } = new List<StudentDto>();
}
