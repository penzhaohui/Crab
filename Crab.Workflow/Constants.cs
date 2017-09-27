using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.Workflow
{
    public class Constants
    {
        public class ApproveStatus
        {
            public const string Submitted = "Submitted";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Cancelled = "Cancelled";
        }

        public class Actions
        {
            public const string Proceed = "StatusProceeded";
            public const string Submit = "Submitted";
            public const string Approve = "Approved";
            public const string Reject = "Rejected";
            public const string Cancel = "Cancelled";
        }
    }
}
