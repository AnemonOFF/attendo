using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Application.Classes.Commands.DeleteClass;
using Attendo.Application.Classes.Commands.SetAttendance;
using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.Classes.Queries.GetClassAttendance;
using Attendo.Application.Classes.Queries.GetClassById;
using Attendo.Application.Classes.Queries.GetClasses;
using Attendo.Application.DTOs.Classes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendo.WebAPI.Controllers;

[ApiController]
[Route("api/v1/classes")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClassesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(ClassesListResponse), 200)]
    public async Task<ActionResult<ClassesListResponse>> GetAll(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] List<int>? group,
        CancellationToken ct)
    {
        from ??= DateTime.MinValue;
        to ??= DateTime.MaxValue;

        var result = await _mediator.Send(new GetClassesQuery
        {
            From = from,
            To = to,
            Group = group
        }, ct);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClassResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ClassResponse>> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetClassByIdQuery { Id = id }, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClassResponse), 200)]
    public async Task<ActionResult<ClassResponse>> Create(
        [FromBody] CreateClassRequest body,
        CancellationToken ct)
    {
        var created = await _mediator.Send(
            new CreateClassCommand(
                body.Name,
                body.Start,
                body.End,
                body.Frequency,
                body.StartTime,
                body.EndTime,
                body.GroupId
            ), ct);

        return Ok(created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClassResponse), 200)]
    public async Task<ActionResult<ClassResponse>> Update(
        int id,
        [FromBody] UpdateClassRequest? body,
        CancellationToken ct)
    {
        var updated = await _mediator.Send(
            new UpdateClassCommand(
                id,
                body?.Name,
                body?.Start,
                body?.End,
                body?.Frequency,
                body?.StartTime,
                body?.EndTime,
                body?.GroupId
            ), ct);

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _mediator.Send(new DeleteClassCommand { Id = id }, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/attendance")]
    [ProducesResponseType(typeof(ClassAttendanceResponse), 200)]
    public async Task<ActionResult<ClassAttendanceResponse>> GetAttendance(int id, CancellationToken ct)
    {
        var attendance = await _mediator.Send(new GetClassAttendanceQuery { ClassId = id }, ct);
        return Ok(attendance);
    }

    [HttpPut("{id:int}/attendance")]
    [ProducesResponseType(typeof(ClassResponse), 200)]
    public async Task<ActionResult<ClassResponse>> SetAttendance(int id, [FromBody] UpdateAttendanceRequest body, CancellationToken ct)
    {
        var result = await _mediator.Send(new SetClassAttendanceCommand(id, body.Attendance), ct);
        return Ok(result);
    }
}
