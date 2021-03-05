using System;
using MassTransit;

namespace persistedworkflowdemo_masstransit.StateMachine.Events
{
    public interface CreateWorkItemEvent : CorrelatedBy<Guid>
    {
        public string BizIdentifier { get; }
        
        public DateTimeOffset ScheduledDate { get; }

    }
}
