using Altura.BidManagement.Application.Common;

namespace Altura.BidManagement.WebApi.Contracts;

public record UpdateBidRequest(
    string Title,
    decimal Amount,
    State State);