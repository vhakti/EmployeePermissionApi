using EmployeePermissionApi.Application.Commands;
using EmployeePermissionApi.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePermissionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
        {
            var permissionId = await _mediator.Send(command);
            return Ok(new { PermissionId = permissionId });
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditPermission([FromBody] EditPermissionCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetPermissions(int employeeId)
        {
            var permissions = await _mediator.Send(new GetPermissionsQuery { EmployeeId = employeeId });
            return Ok(permissions);
        }
    }
}
