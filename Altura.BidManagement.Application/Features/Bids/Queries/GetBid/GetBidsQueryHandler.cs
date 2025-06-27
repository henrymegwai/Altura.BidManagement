using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Mapper;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;

namespace Altura.BidManagement.Application.Features.Bids.Queries.GetBid;

public class GetBidsQueryHandler(IRepository<Bid> bidRepository) : IQueryHandler<GetBidsQuery, Response<List<BidDto>>>
{
    public async Task<Response<List<BidDto>>> Handle(GetBidsQuery request, CancellationToken cancellationToken)
    {
        var bids = await bidRepository.GetAllAsync(null, [], cancellationToken);

        if (!bids.Value.Any())
        {
            return new Response<List<BidDto>>(false, [],  "No bids found.");
        }
        
        return new Response<List<BidDto>>(true, bids.Value
            .Select(bid => bid.MapToDto())
            .ToList(), "Bids retrieved successfully.");
    }
}