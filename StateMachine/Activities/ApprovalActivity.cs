using System.Diagnostics;
using System.Threading.Tasks;
using Automatonymous;

namespace persistedworkflowdemo_masstransit.StateMachine.Activities
{
    public class ApprovalActivity : BoilerplateActivity
     
    {
        public ApprovalActivity() : base(nameof(ApprovalActivity))
        {
        }

        protected override async Task Execute(BehaviorContext<WorkItemState> context)
        {
            Trace.WriteLine($"Approved! {context.Instance.BizIdentifier} by {context.Instance.ApproverName}");
        }
    }
}
