namespace Attendo.Application.DTOs.Students;

public sealed class CreateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public int GroupId { get; set; }
}
