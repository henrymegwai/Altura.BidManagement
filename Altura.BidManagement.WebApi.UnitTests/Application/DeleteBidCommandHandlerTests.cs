using System;
using System.Threading;
using System.Threading.Tasks;
using Altura.BidManagement.Application.Features.Bids.Commands.DeleteBid;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentAssertions;
using FluentResults;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Altura.BidManagement.WebApi.UnitTests.Application;

public class DeleteBidCommandHandlerTests
{
    private readonly IRepository<Altura.BidManagement.Domain.Bids.Bid> _repository;
    private readonly IValidator<DeleteBidCommand> _validator;
    private readonly DeleteBidCommandHandler _sut;

    public DeleteBidCommandHandlerTests()
    {
        _repository = Substitute.For<IRepository<Altura.BidManagement.Domain.Bids.Bid>>();
        _validator = Substitute.For<IValidator<DeleteBidCommand>>();
        var logger = Substitute.For<ILogger<DeleteBidCommandHandler>>();
        _sut = new DeleteBidCommandHandler(_repository, _validator, logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBidIsDeleted()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        var command = new DeleteBidCommand(id);
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.DeleteAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Altura.BidManagement.Domain.Bids.Bid, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok());

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Bid deleted successfully");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var id = Guid.NewGuid();
        var command = new DeleteBidCommand(id);
        
        _validator.ValidateAsync(command, cancellationToken)
            .Returns(new FluentValidation.Results.ValidationResult());
        
        _repository.DeleteAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Altura.BidManagement.Domain.Bids.Bid, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Fail("Failed to delete bid"));

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        result.Status.Should().BeFalse();
        result.Message.Should().Be("Failed to delete bid");
    }
} 