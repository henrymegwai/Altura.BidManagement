using Altura.BidManagement.Application.Features.Bids.Commands.CreateBid;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentAssertions;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using State = Altura.BidManagement.Application.Common.State;

namespace Altura.BidManagement.WebApi.UnitTests.Application;

public class CreateBidCommandHandlerTests
{
    private readonly IRepository<Altura.BidManagement.Domain.Bids.Bid> _repository;
    private readonly IValidator<CreateBidCommand> _validator;
    private readonly CreateBidCommandHandler _sut;

    public CreateBidCommandHandlerTests()
    {
        _repository = Substitute.For<IRepository<Altura.BidManagement.Domain.Bids.Bid>>();
        _validator = Substitute.For<IValidator<CreateBidCommand>>();
       var logger = Substitute.For<ILogger<CreateBidCommandHandler>>();
        _sut = new CreateBidCommandHandler(_repository, _validator, logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBidIsCreated()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateBidCommand("Test Bid", 100, State.Draft.ToString());
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Altura.BidManagement.Domain.Bids.Bid, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Altura.BidManagement.Domain.Bids.Bid)null!);
        
        var id = Guid.NewGuid();
        var bid = new Altura.BidManagement.Domain.Bids.Bid(id,"Test Bid", 100, "Draft", DateTime.UtcNow);
        
        _repository.AddAsync(Arg.Any<Altura.BidManagement.Domain.Bids.Bid>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok(bid));

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Bid created successfully");
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnExistMessage_WhenBidWithTitleExists()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateBidCommand("Test Bid", 100, State.Draft.ToString());
        var id = Guid.NewGuid();
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        var bid = new Altura.BidManagement.Domain.Bids.Bid(id,"Test Bid", 100, "Draft", DateTime.UtcNow);
        _repository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Altura.BidManagement.Domain.Bids.Bid, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(bid);

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Message.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateBidCommand("Test Bid", 100, State.Draft.ToString());
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Altura.BidManagement.Domain.Bids.Bid, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Altura.BidManagement.Domain.Bids.Bid)null!);
        
        _repository.AddAsync(Arg.Any<Altura.BidManagement.Domain.Bids.Bid>(), Arg.Any<CancellationToken>())
            .Returns(Result.Fail<Altura.BidManagement.Domain.Bids.Bid>("DB error"));

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Be("Failed to create bid");
    }
} 