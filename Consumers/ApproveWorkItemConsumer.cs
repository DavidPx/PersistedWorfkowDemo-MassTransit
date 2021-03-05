using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using persistedworkflowdemo_masstransit.StateMachine.Events;

namespace persistedworkflowdemo_masstransit.Consumers
{
    public class ApproveWorkItemConsumer : IConsumer<ApprovalEvent>
    {
        public async Task Consume(ConsumeContext<ApprovalEvent> context)
        {
            await context.Publish(context.Message);
        }
    }
}
