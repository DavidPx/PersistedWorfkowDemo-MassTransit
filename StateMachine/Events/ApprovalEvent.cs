using System;
using MassTransit;

namespace persistedworkflowdemo_masstransit.StateMachine.Events
{
    public interface ApprovalEvent : CorrelatedBy<Guid>
    {
        public string Name { get; }
        public string Notes { get; }
        public bool WasApproved { get; }
    }
}
