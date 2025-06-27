using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Domain.Bids;

namespace Altura.BidManagement.Application.Common.Mapper;

public static class Mappers
{
    public static BidDto MapToDto(this Bid bid)
    {
        return new BidDto(
            Id: bid.Id,
            Title: bid.Title,
            Amount: bid.Amount,
            State: bid.State,
            CreatedAt: bid.CreatedAt);
    }
}