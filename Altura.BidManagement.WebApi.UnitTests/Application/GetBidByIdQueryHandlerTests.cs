using Altura.BidManagement.Application.Features.Bids.Queries.GetBid;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Altura.BidManagement.WebApi.UnitTests.Application;

public class GetBidByIdQueryHandlerTests
{
    private readonly IRepository<Altura.BidManagement.Domain.Bids.Bid> _repository;
    private readonly IValidator<GetBidByIdQuery> _validator;
    private readonly GetBidByIdQueryHandler _sut;

    public GetBidByIdQueryHandlerTests()
    {
        _repository = Substitute.For<IRepository<Altura.BidManagement.Domain.Bids.Bid>>();
       _validator = Substitute.For<IValidator<GetBidByIdQuery>>();
        var logger = Substitute.For<ILogger<GetBidByIdQueryHandler>>();
        _sut = new GetBidByIdQueryHandler(_repository, _validator, logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBidExists()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        
        var query = new GetBidByIdQuery(id);
        
        var bid = new Altura.BidManagement.Domain.Bids.Bid(id,"Bid", 100, "Draft", DateTime.UtcNow) { Id = id };
        
        _validator.ValidateAsync(query, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(bid);

        // Act
        var result = await _sut.Handle(query, cancellationToken);

        // Assert
        result.Status.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Message.Should().Be("Bid retrieved successfully.");
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBidDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        
        var query = new GetBidByIdQuery(id);
        
        _validator.ValidateAsync(query, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Altura.BidManagement.Domain.Bids.Bid)null!);

        // Act
        var result = await _sut.Handle(query, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Be("Bid not found.");
    }
} 