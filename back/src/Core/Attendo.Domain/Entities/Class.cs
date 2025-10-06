namespace Attendo.Domain.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
