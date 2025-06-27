using FluentValidation;

namespace Altura.BidManagement.Application.Features.Bids.Queries.GetBid;

public class GetBidByIdQueryValidator : AbstractValidator<GetBidByIdQuery>
{
    public GetBidByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
} 