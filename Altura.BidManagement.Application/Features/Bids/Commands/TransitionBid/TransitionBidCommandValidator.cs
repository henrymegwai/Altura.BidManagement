using Altura.BidManagement.Application.Common;
using FluentValidation;

namespace Altura.BidManagement.Application.Features.Bids.Commands.TransitionBid;

public class TransitionBidCommandValidator : AbstractValidator<TransitionBidCommand>
{
    public TransitionBidCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.TargetState)
            .NotEmpty().WithMessage("TargetState is required.")
            .Must(state => state == State.Draft.ToString() 
                           || state == State.Submitted.ToString() 
                           || state == State.Approved.ToString() 
                           || state == State.Rejected.ToString())
            .WithMessage("TargetState must be one of the following: Draft, Submitted, Approved, Rejected.");
    }
} 