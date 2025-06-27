using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;

namespace Altura.BidManagement.Application.Features.Bids.Commands.TransitionBid;

public record TransitionBidCommand(Guid Id, string TargetState) : ICommand<Response<BidDto>>;