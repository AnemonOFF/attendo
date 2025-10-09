using Attendo.Application.DTOs.Groups;

namespace Attendo.Application.DTOs.Classes
{
    public class ClassResponse
    {
        public int Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }

        public List<GroupDto> Groups { get; set; } = new();
        public List<int> Attendance { get; set; } = new();
    }
}
