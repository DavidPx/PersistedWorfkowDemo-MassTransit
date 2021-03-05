using System.Diagnostics;
using System.Threading.Tasks;
using Automatonymous;

namespace persistedworkflowdemo_masstransit.StateMachine.Activities
{
    public class DenialActivity : BoilerplateActivity
    {
        

        public DenialActivity() : base(nameof(DenialActivity))
        {
        }

        protected override async Task Execute(BehaviorContext<WorkItemState> context)
        {
            Trace.WriteLine($"Denied! {context.Instance.BizIdentifier} by {context.Instance.ApproverName}");
        }
    }
}
