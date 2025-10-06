using Microsoft.AspNetCore.Mvc;
using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Application.Classes.Commands.DeleteClass;
using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.Classes.Queries;

namespace Attendo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClassesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<ClassDto>> Create([FromBody] CreateClassCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClassDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetClassByIdQuery { Id = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ClassDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetClassesQuery());
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ClassDto>> Update(int id, [FromBody] UpdateClassCommand cmd)
        {
            cmd.Id = id;
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteClassCommand { Id = id });
            return ok ? NoContent() : NotFound();
        }
    }
}
