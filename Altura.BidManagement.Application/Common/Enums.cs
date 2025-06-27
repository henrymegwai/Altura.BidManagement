using System.ComponentModel;

namespace Altura.BidManagement.Application.Common;

public enum State
{
    [Description("Draft")] Draft = 0,
    [Description("Submitted")] Submitted = 1,
    [Description("Approved")] Approved = 2,
    [Description("Rejected")] Rejected = 3,
}