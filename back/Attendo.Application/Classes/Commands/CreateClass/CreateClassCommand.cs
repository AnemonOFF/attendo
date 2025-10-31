using Attendo.Application.DTOs.Classes;
using MediatR;

namespace Attendo.Application.Classes.Commands.CreateClass
{
    public record CreateClassCommand(
    string Name,
    DateOnly Start,
    DateOnly End,
    string Frequency,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int GroupId
    ) : IRequest<ClassResponse>;
}
