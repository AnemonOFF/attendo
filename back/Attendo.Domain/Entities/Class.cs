
namespace Attendo.Domain.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public ICollection<Group> Groups { get; set; } = new List<Group>();

        public ICollection<Student> Attendance { get; set; } = new List<Student>();
    }
}
