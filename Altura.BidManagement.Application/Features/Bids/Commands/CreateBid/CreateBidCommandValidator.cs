using Altura.BidManagement.Application.Common;
using FluentValidation;

namespace Altura.BidManagement.Application.Features.Bids.Commands.CreateBid;

public class CreateBidCommandValidator: AbstractValidator<CreateBidCommand>
{
    public CreateBidCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        
        RuleFor(x => x.State)
            .NotEmpty().WithMessage("TargetState is required.")
            .Must(state => state == State.Draft.ToString() 
                           || state == State.Submitted.ToString() 
                           || state == State.Approved.ToString() 
                           || state == State.Rejected.ToString())
            .WithMessage("State must be one of the following: 0 as Draft, 1 as Submitted, 2 as Approved, 3 as Rejected.");
    }
}