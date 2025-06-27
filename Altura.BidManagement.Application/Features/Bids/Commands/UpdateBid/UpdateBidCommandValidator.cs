using Altura.BidManagement.Application.Common;
using FluentValidation;

namespace Altura.BidManagement.Application.Features.Bids.Commands.UpdateBid;

public class UpdateBidCommandValidator : AbstractValidator<UpdateBidCommand>
{
    public UpdateBidCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .Must(state => state is State.Draft or State.Submitted or State.Approved or State.Rejected)
            .WithMessage("State must be one of the following: Draft, Submitted, Approved, Rejected.");
    }
} 