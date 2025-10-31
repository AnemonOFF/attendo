using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Queries.GetClassById;

public sealed class GetClassByIdQuery : IRequest<ClassResponse?>
{
    public int Id { get; set; }
}
