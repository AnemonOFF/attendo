using Attendo.Application.DTOs.Students;

namespace Attendo.Application.DTOs.Groups;

public class UpdateGroupRequest
{
  public string Title { get; set; } = string.Empty;
  public List<int>? Students { get; set; }
}
