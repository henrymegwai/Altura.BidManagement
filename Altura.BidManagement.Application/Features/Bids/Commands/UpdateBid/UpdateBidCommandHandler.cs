using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Mapper;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Altura.BidManagement.Application.Features.Bids.Commands.UpdateBid;

public class UpdateBidCommandHandler(
    IRepository<Bid> repository,
    IValidator<UpdateBidCommand> validator,
    ILogger<UpdateBidCommandHandler> logger) : ICommandHandler<UpdateBidCommand, Response<BidDto>>
{
    public async Task<Response<BidDto>> Handle(UpdateBidCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidationRequest(request, cancellationToken);
        if (validationResult is not { Status: true })
        {
            return validationResult;
        }
        
        var bid = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (bid == null)
        {
            return new Response<BidDto>(false,null!,$"Bid with Id {request.Id} was not found.");
        }
        
        UpdateBidProperties(bid, request);

        var result = await repository.UpdateAsync(bid, cancellationToken);

        return result.IsFailed ? 
            new Response<BidDto>(false, bid.MapToDto(), 
                $"update request failed: {result.Errors.First().Message}") 
            : new Response<BidDto>(true, bid.MapToDto(), "Bid updated successfully.");
    }
    
    private async Task<Response<BidDto>> ValidationRequest(UpdateBidCommand request,CancellationToken  cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) 
            return new Response<BidDto>(true, null!, "Validation successful");
        
        logger.LogWarning("Validation failed for {Command}: {Errors}", nameof(UpdateBidCommand),
            validationResult.Errors);
        return new Response<BidDto>(
            false,
            null!,
            BidErrors.BidValidationFailed,
            Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
    }

    
    private static void UpdateBidProperties(Bid bid, UpdateBidCommand request)
    {
        bid.Title = request.Title;
        bid.Amount = request.Amount;
        bid.State = request.State.ToString();
        bid.UpdatedAt = DateTime.UtcNow;
    }
}