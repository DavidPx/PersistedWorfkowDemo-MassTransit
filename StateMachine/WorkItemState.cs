using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Automatonymous;

namespace persistedworkflowdemo_masstransit.StateMachine
{
    public class WorkItemState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public int CurrentState { get; set; }

        public string BizIdentifier { get; set; }
        public string ApprovalNotes { get; set; }
        public bool WasApproved { get; set; }
        public string ApproverName { get; set; }
        public DateTimeOffset ScheduledDate { get; set; }

        public Guid? FutureExecutionScheduleTimeoutTokenId { get; private set; }
    }
}
