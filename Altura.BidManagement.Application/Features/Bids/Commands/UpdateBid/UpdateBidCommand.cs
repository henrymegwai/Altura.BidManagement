using Altura.BidManagement.Application.Common;
using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;

namespace Altura.BidManagement.Application.Features.Bids.Commands.UpdateBid;

public record UpdateBidCommand( 
    Guid Id,
    string Title,
    decimal Amount,
    State State
) : ICommand<Response<BidDto>>;