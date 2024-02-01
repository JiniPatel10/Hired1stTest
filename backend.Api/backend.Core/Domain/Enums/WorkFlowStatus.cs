using System;
using System.ComponentModel;

namespace twoladder.Core.Domain.Enums
{
    public enum WorkFlowStatus
    {
		[Description("None")]
		None,
		[Description("Quote Requested")]
        QuoteRequested,
		[Description("Proposed")]
        Proposed,
		[Description("Onboarding")]
        OnBoarding,
		[Description("Creative")]
        Creative,
		[Description("Strategy")]
        Strategy,
		[Description("Live")]
        Live,
		[Description("Inactive")]
        Inactive,
        [Description("Change Requested")]
        ChangeRequested,
        [Description("Change Request Creative")]
        ChangeRequestCreative,
        [Description("Change Request Strategy")]
        ChangeRequestStrategy
    }
}
