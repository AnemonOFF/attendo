using Attendo.Application.DTOs.Groups;
using Attendo.Application.Groups.Commands;
using Attendo.Application.Groups.Commands.UpdateGroup;
using Attendo.Application.Groups.Queries;
using Attendo.Application.Groups.Queries.GetGroupById;
using Attendo.Application.Groups.Queries.GetGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendo.WebAPI.Controllers;

[ApiController]
[Route("api/v1/groups")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;
    public GroupsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(GroupResponse), 200)]
    public async Task<ActionResult<GroupResponse>> Create([FromBody] CreateGroupRequest body)
    {
        var result = await _mediator.Send(new CreateGroupCommand { Title = body.Title });
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GroupResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GroupResponse>> GetById(int id)
    {
        var result = await _mediator.Send(new GetGroupByIdQuery { Id = id });
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GroupsListResponse), 200)]
    public async Task<ActionResult<GroupsListResponse>> GetAll([FromQuery] int? offset, [FromQuery] int? limit)
    {
        var result = await _mediator.Send(new GetGroupsQuery { Offset = offset, Limit = limit });
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GroupResponse), 200)]
    public async Task<ActionResult<GroupResponse>> Update(int id, [FromBody] UpdateGroupRequest? body)
    {
        var result = await _mediator.Send(new UpdateGroupCommand
        {
            Id = id,
            Title = body?.Title,
            Students = body?.Students
        });
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _mediator.Send(new DeleteGroupCommand { Id = id });
        return ok ? NoContent() : NotFound();
    }
}
