using System.Threading.Tasks;
using MassTransit;
using persistedworkflowdemo_masstransit.StateMachine.Events;

namespace persistedworkflowdemo_masstransit.Consumers
{
    public class CreateWorkItemConsumer : IConsumer<CreateWorkItemEvent>
    {
        public async Task Consume(ConsumeContext<CreateWorkItemEvent> context)
        {
            await context.Publish(context.Message);
        }
    }
}
