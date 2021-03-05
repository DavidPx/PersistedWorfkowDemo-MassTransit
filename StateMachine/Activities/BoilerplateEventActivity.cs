using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;

namespace persistedworkflowdemo_masstransit.StateMachine.Activities
{
    public abstract class BoilerplateEventActivity<TEvent> : Activity<WorkItemState, TEvent>
    {
        private readonly string _scopeName;

        protected BoilerplateEventActivity(string scopeName)
        {
            _scopeName = scopeName;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<WorkItemState, TEvent> context, Behavior<WorkItemState, TEvent> next)
        {
            await Execute(context);

            await next.Execute(context).ConfigureAwait(false);
        }

        protected abstract Task Execute(BehaviorContext<WorkItemState, TEvent> context);

        public virtual Task Faulted<TException>(BehaviorExceptionContext<WorkItemState, TEvent, TException> context, Behavior<WorkItemState, TEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope(_scopeName);
        }
    }
}
