namespace Attendo.Application.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
