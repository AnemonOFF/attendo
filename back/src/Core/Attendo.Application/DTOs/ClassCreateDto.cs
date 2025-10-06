namespace Attendo.Application.DTOs
{
    public class ClassCreateDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
