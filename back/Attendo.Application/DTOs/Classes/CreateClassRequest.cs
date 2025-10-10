namespace Attendo.Application.DTOs.Classes;

public class CreateClassRequest
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public int GroupId { get; set; }
}
