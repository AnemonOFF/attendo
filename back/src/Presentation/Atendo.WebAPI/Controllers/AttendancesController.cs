using Microsoft.AspNetCore.Mvc;
using MediatR;
using Atendo.Application.DTOs;
using Atendo.Application.Attendances.Commands.CreateAttendance;
using Atendo.Application.Attendances.Commands.UpdateAttendance;
using Atendo.Application.Attendances.Commands.DeleteAttendance;
using Atendo.Application.Attendances.Queries.GetAttendanceById;
using Atendo.Application.Attendances.Queries.GetAttendances;
using Atendo.Application.Attendances.Queries.GetAttendancesByEvent;

namespace Atendo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AttendancesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<AttendanceDto>> Create([FromBody] CreateAttendanceCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AttendanceDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetAttendanceByIdQuery { Id = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AttendanceDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAttendancesQuery());
            return Ok(result);
        }

        [HttpGet("by-event/{eventId:int}")]
        public async Task<ActionResult<IReadOnlyList<AttendanceDto>>> GetByEvent(int eventId)
        {
            var result = await _mediator.Send(new GetAttendancesByEventQuery { EventId = eventId });
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<AttendanceDto>> Update(int id, [FromBody] UpdateAttendanceCommand cmd)
        {
            cmd.Id = id;
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteAttendanceCommand { Id = id });
            return ok ? NoContent() : NotFound();
        }
    }
}
