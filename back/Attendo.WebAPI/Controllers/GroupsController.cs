using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.Groups.Commands;
using Attendo.Application.Groups.Queries;

namespace Attendo.WebAPI.Controllers;

[ApiController]
[Route("groups")]
[Authorize]
public class GroupsController : ControllerBase
{
  private readonly IMediator _mediator;
  public GroupsController(IMediator mediator) => _mediator = mediator;

  [HttpPost]
  public async Task<ActionResult<GroupDto>> Create([FromBody] CreateGroupRequest body)
  {
    var result = await _mediator.Send(new CreateGroupCommand { Title = body.Title });
    return Ok(result);
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<GroupDto>> GetById(int id)
  {
    var result = await _mediator.Send(new GetGroupByIdQuery { Id = id });
    return result is null ? NotFound() : Ok(result);
  }

  [HttpGet]
  public async Task<ActionResult<GroupsListResponse>> GetAll([FromQuery] int? offset, [FromQuery] int? limit)
  {
    var result = await _mediator.Send(new GetGroupsQuery
    {
      Offset = offset,
      Limit = limit
    });
    return Ok(result);
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult<GroupDto>> Update(int id, [FromBody] UpdateGroupRequest body)
  {
    var result = await _mediator.Send(new UpdateGroupCommand
    {
      Id = id,
      Title = body.Title,
      Students = body.Students
    });
    return Ok(result);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    var ok = await _mediator.Send(new DeleteGroupCommand { Id = id });
    return ok ? NoContent() : NotFound();
  }
}
