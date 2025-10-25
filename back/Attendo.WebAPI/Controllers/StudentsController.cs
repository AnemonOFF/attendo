using Attendo.Application.DTOs.Students;
using Attendo.Application.Students.Commands;
using Attendo.Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Attendo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudentsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<StudentsListResponse>> GetAll()
        {
            var result = await _mediator.Send(new GetStudentsQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetStudentByIdQuery { Id = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(CreateStudentCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<StudentDto>> Update(int id, UpdateStudentCommand cmd)
        {
            cmd.Id = id;
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteStudentCommand { Id = id });
            return ok ? NoContent() : NotFound();
        }
    }
}
