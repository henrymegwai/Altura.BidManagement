using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Application.Features.Bids.Commands.CreateBid;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Altura.BidManagement.Application.Features.Bids.Commands.DeleteBid;

public class DeleteBidCommandHandler(
    IRepository<Bid> repository,
    IValidator<DeleteBidCommand> validator,
    ILogger<DeleteBidCommandHandler> logger) 
    : ICommandHandler<DeleteBidCommand, Response<Unit>>
{
    public async Task<Response<Unit>> Handle(DeleteBidCommand request, CancellationToken cancellationToken)
    {
        
        var validationResult = await ValidationRequest(request, cancellationToken);
        if (validationResult is not { Status: true })
        {
            return validationResult;
        }
        
        var result = await repository.DeleteAsync(b => b.Id == request.Id, cancellationToken);
        
        return result.IsFailed ? 
            new Response<Unit>(false, Unit.Value, "Failed to delete bid") : 
            new Response<Unit>(true, Unit.Value, "Bid deleted successfully");
    }
    
    private async Task<Response<Unit>> ValidationRequest(DeleteBidCommand request,CancellationToken  cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) 
            return new Response<Unit>(true, Unit.Value, "Validation successful");
        
        logger.LogWarning("Validation failed for {Command}: {Errors}", nameof(CreateBidCommand),
            validationResult.Errors);
        return new Response<Unit>(
            false!,
            Unit.Value, 
            BidErrors.BidValidationFailed,
            Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
    }
}