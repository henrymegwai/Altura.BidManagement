using Altura.BidManagement.Application.Common.Interfaces;
using Altura.BidManagement.Application.Common.Mapper;
using Altura.BidManagement.Application.Common.Models;
using Altura.BidManagement.Application.Dto;
using Altura.BidManagement.Domain.Bids;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Altura.BidManagement.Application.Features.Bids.Commands.CreateBid;

public class CreateBidCommandHandler(IRepository<Bid> bidRepository,
     IValidator<CreateBidCommand> validator,
     ILogger<CreateBidCommandHandler> logger)
    : ICommandHandler<CreateBidCommand, Response<BidDto>>
{
    public async Task<Response<BidDto>> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidationRequest(request, cancellationToken);
        if (validationResult is not { Status: true })
        {
            return validationResult;
        }
        
        if (await BidExistsAsync(request.Title, cancellationToken))
        {
            return new Response<BidDto>(true, null!, BidErrors.BidExist);
        }
        
        var bid = CreateBidFromRequest(request);

        var addBidResult = await AddBidAsync(bid, cancellationToken);

        return addBidResult;
    }
    
    private async Task<Response<BidDto>> ValidationRequest(CreateBidCommand request,CancellationToken  cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid) 
            return new Response<BidDto>(true, null!, "Validation successful");
        
        logger.LogWarning("Validation failed for {Command}: {Errors}", nameof(CreateBidCommand),
            validationResult.Errors);
        return new Response<BidDto>(
            false,
            null!,
            BidErrors.BidValidationFailed,
            Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
    }
    
    private async Task<bool> BidExistsAsync(string title, CancellationToken cancellationToken)
    {
        var bid = await bidRepository.GetAsync(b => b.Title == title, cancellationToken);
        return bid != null;
    }
    
    private Bid CreateBidFromRequest(CreateBidCommand request)
    {
        return new Bid(
            Guid.NewGuid(),
            request.Title,
            request.Amount,
            request.State.ToString(),
            DateTime.UtcNow);
    }
    
    private async Task<Response<BidDto>> AddBidAsync(Bid bid, CancellationToken cancellationToken)
    {
        var addBidResult = await bidRepository.AddAsync(bid, cancellationToken);

        if (!addBidResult.IsFailed || !addBidResult.Errors.Any())
            return new Response<BidDto>(true, addBidResult.Value.MapToDto(),
                "Bid created successfully");

        logger.LogError("Failed to create bid: {Errors}", addBidResult.Errors);
        return new Response<BidDto>(false, null!, "Failed to create bid");
    }
}