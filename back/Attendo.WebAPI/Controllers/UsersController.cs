using Attendo.Application.DTOs.Users;
using Attendo.Application.Users.Commands.CreateUser;
using Attendo.Application.Users.Commands.DeleteUser;
using Attendo.Application.Users.Commands.UpdateUser;
using Attendo.Application.Users.Queries.GetUserById;
using Attendo.Application.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Attendo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<UsersListResponse>> GetAll()
        {
            var result = await _mediator.Send(new GetUsersQuery());
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UpdateUserCommand cmd)
        {
            cmd.Id = id;
            var result = await _mediator.Send(cmd);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteUserCommand { Id = id });
            return ok ? NoContent() : NotFound();
        }
    }
}
