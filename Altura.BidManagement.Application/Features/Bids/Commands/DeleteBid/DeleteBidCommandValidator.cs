using FluentValidation;

namespace Altura.BidManagement.Application.Features.Bids.Commands.DeleteBid;

public class DeleteBidCommandValidator : AbstractValidator<DeleteBidCommand>
{
    public DeleteBidCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
} 