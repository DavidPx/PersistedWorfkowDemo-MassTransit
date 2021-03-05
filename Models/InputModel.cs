using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace persistedworkflowdemo_masstransit.Models
{
    public class InputModel : IValidatableObject
    {
        public string BizIdentifier { get; set; }
        public string WhoSent { get; set; }
        public DateTimeOffset ScheduledDate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ScheduledDate < DateTimeOffset.Now)
            {
                yield return new ValidationResult("Scheduled date in the past", new[] { nameof(ScheduledDate) });
            }
        }
    }
}
