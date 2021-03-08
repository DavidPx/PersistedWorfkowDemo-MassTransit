using System;
using System.Diagnostics;
using Automatonymous;
using MassTransit;
using persistedworkflowdemo_masstransit.StateMachine.Activities;
using persistedworkflowdemo_masstransit.StateMachine.Events;

namespace persistedworkflowdemo_masstransit.StateMachine
{
    public interface FutureExecutionScheduleTimeoutExpired
    {
        Guid CorrelationId { get; set; }
    }
    public class WorkItemStateMachine : MassTransitStateMachine<WorkItemState>
    {
        public State NewItem { get; private set; }
        public State WaitingForApproval { get; private set; }
        public State ApprovedWaitingForExecution { get; private set; }
        public State Executed { get; private set; }

        public Event<ApprovalEvent> ApproveWorkItem { get; private set; }
        public Event<CreateWorkItemEvent> CreateWorkItem { get; private set; }
        public Schedule<WorkItemState, FutureExecutionScheduleTimeoutExpired> FutureExecutionSchedule { get; private set; }
        
        public WorkItemStateMachine()
        {
            InstanceState(x => x.CurrentState, WaitingForApproval, ApprovedWaitingForExecution, Executed);
            Event(() => CreateWorkItem);
            Event(() => ApproveWorkItem);
            Schedule(() => FutureExecutionSchedule, instance => instance.FutureExecutionScheduleTimeoutTokenId, s =>
            {
                s.Received = r => r.CorrelateById(context => context.Message.CorrelationId);
            });

            Initially(
                When(CreateWorkItem)
                    .Then(x =>
                    {
                        Trace.WriteLine("Created work item!");
                        x.Instance.ScheduledDate = x.Data.ScheduledDate;
                        x.Instance.BizIdentifier = x.Data.BizIdentifier;
                    })
                    .TransitionTo(WaitingForApproval)
                );

            During(WaitingForApproval,
                /*
                 * If I skip this Ignore I get an "infinite loop" of this exception
                 *
                 *  The CreateWorkItem event is not handled during the WaitingForApproval state for the WorkItemStateMachine state machine
                 */
                Ignore(CreateWorkItem),
                When(ApproveWorkItem, filter => filter.Data.WasApproved == false)
                    .Then(x =>
                    {
                        Trace.WriteLine("Denied");
                        x.Instance.ApproverName = x.Data.Name;
                        x.Instance.ApprovalNotes = x.Data.Notes;
                    })
                    .Activity(x => x.OfInstanceType<DenialActivity>())
                    .Finalize(),
                When(ApproveWorkItem, filter => filter.Data.WasApproved == true)
                    .Then(x =>
                    {
                        Trace.WriteLine($"Got approval!  Scheduling execution for {x.Instance.ScheduledDate:g}");
                        x.Instance.ApproverName = x.Data.Name;
                        x.Instance.ApprovalNotes = x.Data.Notes;
                    })
                    .Schedule(
                        FutureExecutionSchedule,
                        context => context.Init<FutureExecutionScheduleTimeoutExpired>(new { context.Instance.CorrelationId }),
                        context => context.Instance.ScheduledDate.UtcDateTime
                    )
                    .TransitionTo(ApprovedWaitingForExecution)
            );
            During(ApprovedWaitingForExecution, 
                When(FutureExecutionSchedule.Received)
                    .Activity(x => x.OfInstanceType<ApprovalActivity>())
                    .Finalize(),
                /*
                 * If these are skipped I get an "infinite loop" of these two exceptions:
                 *  The ApproveWorkItem event is not handled during the ApprovedWaitingForExecution state for the WorkItemStateMachine state machine
                 *  The CreateWorkItem event is not handled during the ApprovedWaitingForExecution state for the WorkItemStateMachine state machine
                 */

                Ignore(ApproveWorkItem),
                Ignore(CreateWorkItem)
                );
            
            // Without this line I get the same "infinite loop" pair of exceptions as above
            //During(Final, Ignore(ApproveWorkItem), Ignore(CreateWorkItem));
            
        }
    }
}
