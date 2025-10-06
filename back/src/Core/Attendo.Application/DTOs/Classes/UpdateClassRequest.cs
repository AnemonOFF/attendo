namespace Attendo.Application.DTOs.Classes;

public class UpdateClassRequest
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public int GroupId { get; set; }
}
