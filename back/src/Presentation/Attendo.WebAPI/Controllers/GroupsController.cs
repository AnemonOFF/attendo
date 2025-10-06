using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.Groups.Commands;
using Attendo.Application.Groups.Queries;
using Attendo.Application.DTOs.Groups;

namespace Attendo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GroupsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<GroupDto>> Create([FromBody] CreateGroupCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GroupDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetGroupByIdQuery { Id = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GroupDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetGroupsQuery());
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GroupDto>> Update(int id, [FromBody] UpdateGroupCommand cmd)
        {
            cmd.Id = id;
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteGroupCommand { Id = id });
            return ok ? NoContent() : NotFound();
        }
    }
}
