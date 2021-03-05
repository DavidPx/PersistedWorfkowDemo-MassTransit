using System;

namespace persistedworkflowdemo_masstransit.Models
{
    public class ApprovalModel
    {
        public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}
