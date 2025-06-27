using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Models;
using MediatR;

namespace Altura.BidManagement.Application.Features.Bids.Commands.DeleteBid;

public record DeleteBidCommand(Guid Id) : ICommand<Response<Unit>>;