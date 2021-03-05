using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;

namespace persistedworkflowdemo_masstransit.StateMachine.Activities
{
    public abstract class BoilerplateActivity : Activity<WorkItemState>
    {
        private readonly string _scopeName;

        protected BoilerplateActivity(string scopeName)
        {
            _scopeName = scopeName;
        }
        public void Probe(ProbeContext context)
        {
            context.CreateScope(_scopeName);
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected abstract Task Execute(BehaviorContext<WorkItemState> context);


        public async Task Execute(BehaviorContext<WorkItemState> context, Behavior<WorkItemState> next)
        {
            await Execute(context);

            await next.Execute(context).ConfigureAwait(false);
        }

        public async Task Execute<T>(BehaviorContext<WorkItemState, T> context, Behavior<WorkItemState, T> next)
        {
            await Execute(context);

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<WorkItemState, TException> context, Behavior<WorkItemState> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public Task Faulted<T, TException>(BehaviorExceptionContext<WorkItemState, T, TException> context, Behavior<WorkItemState, T> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }
}
