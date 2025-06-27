using System;
using System.Threading;
using System.Threading.Tasks;
using Altura.BidManagement.Application.Features.Bids.Commands.TransitionBid;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentAssertions;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using State = Altura.BidManagement.Application.Common.State;

namespace Altura.BidManagement.WebApi.UnitTests.Application;

public class TransitionBidCommandHandlerTests
{
    private readonly IRepository<Altura.BidManagement.Domain.Bids.Bid> _repository;
    private readonly IValidator<TransitionBidCommand> _validator;
    private readonly TransitionBidCommandHandler _sut;

    public TransitionBidCommandHandlerTests()
    {
        _repository = Substitute.For<IRepository<Altura.BidManagement.Domain.Bids.Bid>>();
        _validator = Substitute.For<IValidator<TransitionBidCommand>>();
        var logger = Substitute.For<ILogger<TransitionBidCommandHandler>>();
        _sut = new TransitionBidCommandHandler(_repository, _validator, logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenTransitionIsValid()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var id = Guid.NewGuid();
        var command = new TransitionBidCommand(id, State.Submitted.ToString());
        var bid = new Altura.BidManagement.Domain.Bids.Bid(id,"Bid", 100, State.Draft.ToString(), DateTime.UtcNow) { Id = id };
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(bid);
        _repository.UpdateAsync(Arg.Any<Altura.BidManagement.Domain.Bids.Bid>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok(bid));

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Bid transitioned successfully.");
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBidDoesNotExist()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var id = Guid.NewGuid();
        var command = new TransitionBidCommand(id, State.Submitted.ToString());
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Altura.BidManagement.Domain.Bids.Bid)null!);

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Contain("not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTransitionIsInvalid()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var id = Guid.NewGuid();
        var command = new TransitionBidCommand(id, State.Approved.ToString()); // Invalid from Draft to Approved
        var bid = new Altura.BidManagement.Domain.Bids.Bid(id,"Bid", 100, State.Draft.ToString(), DateTime.UtcNow) { Id = id };
       
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(bid);

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().NotBeNull();
        result.Message.Should().Contain("Invalid transition");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var id = Guid.NewGuid();
        var command = new TransitionBidCommand(id, State.Submitted.ToString());
        var bid = new Altura.BidManagement.Domain.Bids.Bid(id,"Bid", 100, State.Draft.ToString(), DateTime.UtcNow) { Id = id };
       
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(bid);
        _repository.UpdateAsync(Arg.Any<Altura.BidManagement.Domain.Bids.Bid>(), Arg.Any<CancellationToken>())
            .Returns(Result.Fail<Altura.BidManagement.Domain.Bids.Bid>("update request failed: DB error"));

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().NotBeNull();
        result.Message.Should().Contain("update request failed");
    }
} 