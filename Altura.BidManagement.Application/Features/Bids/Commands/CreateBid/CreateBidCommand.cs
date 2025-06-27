using Altura.BidManagement.Application.Common;
using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;

namespace Altura.BidManagement.Application.Features.Bids.Commands.CreateBid;

public record CreateBidCommand( 
    string Title,
    decimal Amount,
    string State
) : ICommand<Response<BidDto>>;