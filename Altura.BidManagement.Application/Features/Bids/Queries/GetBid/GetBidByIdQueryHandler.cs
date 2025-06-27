using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Mapper;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Altura.BidManagement.Application.Features.Bids.Queries.GetBid;

public class GetBidByIdQueryHandler(
    IRepository<Bid> repository,
    IValidator<GetBidByIdQuery> validator,
    ILogger<GetBidByIdQueryHandler> logger ) 
    : IQueryHandler<GetBidByIdQuery, Response<BidDto>>
{
    public async Task<Response<BidDto>> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidationRequest(request, cancellationToken);
        if (validationResult is not { Status: true })
        {
            return validationResult;
        }
        
        return await HandleAsync(request, cancellationToken);
    }
    
    private async Task<Response<BidDto>> ValidationRequest(GetBidByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) 
            return new Response<BidDto>(true, null!, "Validation successful");
        
        logger.LogWarning("Validation failed for {Command}: {Errors}", nameof(GetBidByIdQueryHandler),
            validationResult.Errors);
        return new Response<BidDto>(
            false,
            null!,
            BidErrors.BidValidationFailed,
            Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
    }
    
    private async Task<Response<BidDto>> HandleAsync(GetBidByIdQuery request, CancellationToken cancellationToken)
    {
        var bid = await repository.GetByIdAsync(request.Id, cancellationToken);

        return bid == null ? 
            new Response<BidDto>(false, null!, "Bid not found.") 
            : new Response<BidDto>(true, bid.MapToDto(), "Bid retrieved successfully.");
    }
}