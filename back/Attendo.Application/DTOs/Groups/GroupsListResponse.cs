namespace Attendo.Application.DTOs.Groups;

public class GroupsListResponse
{
    public int Total { get; set; }
    public List<GroupResponse> Items { get; set; } = new();
}
