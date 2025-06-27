using Altura.BidManagement.Application.Common;
using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Mapper;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Altura.BidManagement.Application.Features.Bids.Commands.TransitionBid;

public class TransitionBidCommandHandler(
    IRepository<Bid> repository,
    IValidator<TransitionBidCommand> validator,
    ILogger<TransitionBidCommandHandler> logger)
    : ICommandHandler<TransitionBidCommand, Response<BidDto>>
{
    public async Task<Response<BidDto>> Handle(TransitionBidCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidationRequest(request, cancellationToken);
        if (validationResult is not { Status: true })
        {
            return validationResult;
        }
        
        return await HandleTransitionAsync(request, cancellationToken);
    }
    
    private async Task<Response<BidDto>> HandleTransitionAsync(TransitionBidCommand request, CancellationToken cancellationToken)
    {
        var bid = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (bid == null)
        {
            return new Response<BidDto>(false, null!, $"Bid with Id {request.Id} not found.");
        }

        if (!IsValidTransition(bid.State, request.TargetState))
        {
            return new Response<BidDto>(false,bid.MapToDto(),
                $"Invalid transition from {bid.State} to {request.TargetState}.");
        }
        
        UpdateBidState(bid, request.TargetState);

        var result = await repository.UpdateAsync(bid, cancellationToken);
        
        return result.IsFailed ? 
            new Response<BidDto>(false, bid.MapToDto(), 
                $"update request failed: {result.Errors.First().Message}") 
            : new Response<BidDto>(true, bid.MapToDto(), "Bid transitioned successfully.");
    }
    
    private async Task<Response<BidDto>> ValidationRequest(TransitionBidCommand request,CancellationToken  cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) 
            return new Response<BidDto>(true, null!, "Validation successful");
        
        logger.LogWarning("Validation failed for {Command}: {Errors}", nameof(TransitionBidCommand),
            validationResult.Errors);
        return new Response<BidDto>(
            false,
            null!,
            BidErrors.BidValidationFailed,
            Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
    }
    
    private static bool IsValidTransition(string currentState, string targetState)
    {
        return (currentState == State.Draft.ToString() && targetState == State.Submitted.ToString()) ||
               (currentState == State.Submitted.ToString() && targetState == State.Approved.ToString());
    }
    
    private static void UpdateBidState(Bid bid, string targetState)
    {
        bid.UpdatedAt = DateTime.UtcNow;
        bid.State = targetState;
    }
}