using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Altura.BidManagement.Application.Features.Bids.Commands.UpdateBid;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentAssertions;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using State = Altura.BidManagement.Application.Common.State;

namespace Altura.BidManagement.WebApi.UnitTests.Application;

public class UpdateBidCommandHandlerTests
{
    private readonly IRepository<Altura.BidManagement.Domain.Bids.Bid> _repository;
    private readonly IValidator<UpdateBidCommand> _validator;
    private readonly UpdateBidCommandHandler _sut;

    public UpdateBidCommandHandlerTests()
    {
        _repository = Substitute.For<IRepository<Altura.BidManagement.Domain.Bids.Bid>>();
        _validator = Substitute.For<IValidator<UpdateBidCommand>>();
        var logger = Substitute.For<ILogger<UpdateBidCommandHandler>>();
        _sut = new UpdateBidCommandHandler(_repository, _validator, logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBidIsUpdated()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        var command = new UpdateBidCommand(id, "Updated Bid", 200, State.Approved);
        var existingBid = new Altura.BidManagement.Domain.Bids.Bid(id, "Old Bid", 100, "Draft", DateTime.UtcNow) { Id = id };
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(existingBid);
        _repository.UpdateAsync(Arg.Any<Altura.BidManagement.Domain.Bids.Bid>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok(existingBid));

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Bid updated successfully.");
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBidDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        var command = new UpdateBidCommand(id, "Updated Bid", 200, State.Approved);
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Altura.BidManagement.Domain.Bids.Bid)null!);

        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Contain("was not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        var command = new UpdateBidCommand(id, "Updated Bid", 200, State.Approved);
        var existingBid = new Altura.BidManagement.Domain.Bids.Bid(id, "Old Bid", 100, "Draft", DateTime.UtcNow) { Id = id };
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(existingBid);
        _repository.UpdateAsync(Arg.Any<Altura.BidManagement.Domain.Bids.Bid>(), Arg.Any<CancellationToken>())
            .Returns(Result.Fail<Altura.BidManagement.Domain.Bids.Bid>("update request failed: DB error"));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().NotBeNull();
        result.Message.Should().Contain("update request failed");
    }
} 