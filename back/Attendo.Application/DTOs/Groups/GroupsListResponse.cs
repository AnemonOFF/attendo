namespace Attendo.Application.DTOs.Groups;

public class GroupsListResponse
{
    public IList<GroupResponse> Items { get; set; } = new List<GroupResponse>();
}
