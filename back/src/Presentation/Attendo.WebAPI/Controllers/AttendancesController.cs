using Microsoft.AspNetCore.Mvc;
using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Commands.CreateAttendance;
using Attendo.Application.Attendances.Commands.UpdateAttendance;
using Attendo.Application.Attendances.Commands.DeleteAttendance;
using Attendo.Application.Attendances.Queries.GetAttendanceById;
using Attendo.Application.Attendances.Queries.GetAttendances;
using Attendo.Application.Attendances.Queries.GetAttendancesByClass;

namespace Attendo.WebAPI.Controllers
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

        [HttpGet("by-class/{classId:int}")]
        public async Task<ActionResult<IReadOnlyList<AttendanceDto>>> GetByClass(int classId)
        {
            var result = await _mediator.Send(new GetAttendancesByClassQuery { ClassId = classId });
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
