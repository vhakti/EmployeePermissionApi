using Confluent.Kafka;
using Elastic.Clients.Elasticsearch.Security;
using EmployeePermissionApi.Application.brokers;
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
        private readonly KafkaProducerService _kafkaProducer;
        public PermissionController(IMediator mediator, KafkaProducerService kafkaProducer)
        {
            _mediator = mediator;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
        {
            try
            {
                var permissionId = await _mediator.Send(command);
                await _kafkaProducer.SendMessageAsync(Guid.NewGuid().ToString(), "Name operation:Request");
                return Ok(new { PermissionId = permissionId });
            }
            catch(Exception sxm) { return BadRequest(sxm.Message); }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditPermission([FromBody] EditPermissionCommand command)
        {
            try { 
            await _mediator.Send(command);
                await _kafkaProducer.SendMessageAsync(Guid.NewGuid().ToString(), "Name operation:Edit");
                return NoContent();
            }
            catch (Exception sxm) { return BadRequest(sxm.Message); }

        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetPermissions(int employeeId)
        {
            try { 
            var permissions = await _mediator.Send(new GetPermissionsQuery { EmployeeId = employeeId });
                await _kafkaProducer.SendMessageAsync(Guid.NewGuid().ToString(), "Name operation:Get");
                return Ok(permissions);
            }
            catch (Exception sxm) { return BadRequest(sxm.Message); }
        }
    }
}
