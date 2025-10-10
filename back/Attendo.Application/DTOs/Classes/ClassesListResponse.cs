namespace Attendo.Application.DTOs.Classes;

public class ClassesListResponse
{
    public IList<ClassResponse> Items { get; set; } = new List<ClassResponse>();
}
