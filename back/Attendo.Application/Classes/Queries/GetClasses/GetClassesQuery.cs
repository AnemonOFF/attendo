using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Queries.GetClasses;

public class GetClassesQuery : IRequest<ClassesListResponse>
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public List<int>? Group { get; set; }
}
