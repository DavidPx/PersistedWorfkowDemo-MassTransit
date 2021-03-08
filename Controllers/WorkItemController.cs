using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MassTransit;
using persistedworkflowdemo_masstransit.Models;
using persistedworkflowdemo_masstransit.StateMachine.Events;

namespace persistedworkflowdemo_masstransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public WorkItemController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InputModel input)
        {
            var newCorrelationId = Guid.NewGuid();

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-work-item"));

            await sendEndpoint.Send<CreateWorkItemEvent>(new
            {
                CorrelationId = newCorrelationId,
                input.BizIdentifier,
                input.ScheduledDate
            });

            return Accepted(new { correlationId = newCorrelationId });
        }

        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] ApprovalModel input)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:approve-work-item"));

            await sendEndpoint.Send<ApprovalEvent>(
            new
            {
                input.CorrelationId,
                input.Name,
                input.Notes,
                WasApproved = true
            });

            return Ok();
        }

        [HttpPost("deny")]
        public async Task<IActionResult> Deny([FromBody] ApprovalModel input)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:approve-work-item"));

            await sendEndpoint.Send<ApprovalEvent>(
                new
                {
                    input.CorrelationId,
                    input.Name,
                    input.Notes,
                    WasApproved = false
                });

            return Ok();
        }
    }
}
