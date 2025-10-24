using MediatR;
using Microsoft.AspNetCore.Mvc;
using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.Classes.Commands.SetAttendance;
using Attendo.Application.Classes.Queries;
using Attendo.Application.DTOs.Classes;

namespace Attendo.WebAPI.Controllers
{
    [ApiController]
    [Route("classes")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClassesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<ClassesListResponse>> GetAll(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetClassesQuery(), ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClassDto>> GetById(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetClassByIdQuery { Id = id }, ct);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ClassDto>> Create([FromBody] CreateClassCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ClassDto>> Update(int id, [FromBody] UpdateClassCommand command, CancellationToken ct)
        {
            if (id != command.Id) return BadRequest("ID in URL and body do not match.");

            var result = await _mediator.Send(command, ct);
            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id:int}/attendance")]
        public async Task<ActionResult<ClassResponse>> SetAttendance(
            int id,
            [FromBody] UpdateAttendanceRequest request,
            CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new SetClassAttendanceCommand
                {
                    ClassId = id,
                    StudentIds = request.StudentIds
                }, ct);

                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
