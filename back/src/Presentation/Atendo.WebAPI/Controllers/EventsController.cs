using Microsoft.AspNetCore.Mvc;
using MediatR;
using Atendo.Application.DTOs;
using Atendo.Application.Events.Commands.CreateEvent;
using Atendo.Application.Events.Commands.DeleteEvent;
using Atendo.Application.Events.Commands.UpdateEvent;
using Atendo.Application.Events.Queries;

namespace Atendo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetEventByIdQuery { Id = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<EventDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetEventsQuery());
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EventDto>> Update(int id, [FromBody] UpdateEventCommand cmd)
        {
            cmd.Id = id;
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteEventCommand { Id = id });
            return ok ? NoContent() : NotFound();
        }
    }
}
