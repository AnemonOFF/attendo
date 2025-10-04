namespace Atendo.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;

        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public List<Attendance> Attendances { get; set; } = new();
    }
}
