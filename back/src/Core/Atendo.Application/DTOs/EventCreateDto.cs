namespace Atendo.Application.DTOs
{
    public class EventCreateDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
