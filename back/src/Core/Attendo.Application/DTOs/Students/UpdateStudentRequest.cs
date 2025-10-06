namespace Attendo.Application.DTOs.Students;

public sealed class UpdateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public int GroupId { get; set; }
}
