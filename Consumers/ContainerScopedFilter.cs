using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using persistedworkflowdemo_masstransit.StateMachine.Events;

namespace persistedworkflowdemo_masstransit.Consumers
{
    public class ContainerScopedFilter :
        IFilter<ConsumeContext<CreateWorkItemEvent>>
    {
        public Task Send(ConsumeContext<CreateWorkItemEvent> context, IPipe<ConsumeContext<CreateWorkItemEvent>> next)
        {
            var provider = context.GetPayload<IServiceProvider>();

            Trace.WriteLine("Filter ran");

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}