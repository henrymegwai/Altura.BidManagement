using Altura.BidManagement.Application.Common;

namespace Altura.BidManagement.WebApi.Contracts;

public record CreateBidRequest(
    string Title,
    decimal Amount,
    State State);
