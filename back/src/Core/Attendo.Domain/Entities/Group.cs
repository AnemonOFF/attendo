namespace Attendo.Domain.Entities;

public class Group
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public List<Student> Students { get; set; } = new();
    public List<Class> Events { get; set; } = new();
}
