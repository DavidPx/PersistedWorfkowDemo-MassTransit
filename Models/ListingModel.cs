using System;

namespace persistedworkflowdemo_masstransit.Models
{
    public class ListingModel
    {
        public Guid CorrelationId { get; set; }
        public string BizIdentifier { get; set; }
        public Approval Approval { get; set; }
        public DateTimeOffset WhenStarted { get; set; }
        public DateTimeOffset? WhenFinished { get; set; }
        public DateTimeOffset ScheduledDate { get; set; }
        public string Status { get; set; }
        public string WaitingFor { get; set; }
    }

    public class Approval
    {
        public bool WasApproved { get; set; }
        public string ApproverName { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset WhenApproved { get; set; }
    }
}
