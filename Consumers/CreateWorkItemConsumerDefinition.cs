using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using persistedworkflowdemo_masstransit.StateMachine.Events;

namespace persistedworkflowdemo_masstransit.Consumers
{
    public class CreateWorkItemConsumerDefinition : ConsumerDefinition<CreateWorkItemConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateWorkItemConsumer> consumerConfigurator)
        {
            consumerConfigurator.Message<CreateWorkItemEvent>(m => m.UseFilter(new ContainerScopedFilter()));
        }
    }
}
