using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Altura.BidManagement.Application.Features.Bids.Queries.GetBid;
using Altura.BidManagement.Infrastructure.Persistence.Common.Repository;
using FluentAssertions;
using FluentResults;
using NSubstitute;
using Xunit;

namespace Altura.BidManagement.WebApi.UnitTests.Application;

public class GetBidsQueryHandlerTests
{
    private readonly IRepository<Altura.BidManagement.Domain.Bids.Bid> _repository;
    private readonly GetBidsQueryHandler _sut;

    public GetBidsQueryHandlerTests()
    {
        _repository = Substitute.For<IRepository<Altura.BidManagement.Domain.Bids.Bid>>();
        _sut = new GetBidsQueryHandler(_repository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBidsExist()
    {
        // Arrange
        var bids = new List<Altura.BidManagement.Domain.Bids.Bid>
        {
            new(Guid.NewGuid(),"Bid1", 100, "Draft", DateTime.UtcNow) { Id = Guid.NewGuid() },
            new(Guid.NewGuid(),"Bid2", 200, "Submitted", DateTime.UtcNow) { Id = Guid.NewGuid() }
        };
        _repository.GetAllAsync(Arg.Any<Expression<Func<Domain.Bids.Bid, bool>>?>(), Arg.Any<Expression<Func<Domain.Bids.Bid, object>>[]?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok(bids));

        // Act
        var result = await _sut.Handle(new GetBidsQuery(), CancellationToken.None);

        // Assert
        result.Status.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.Message.Should().Be("Bids retrieved successfully.");
    }

    [Fact]
    public async Task Handle_ShouldReturnNoBidsFound_WhenNoBidsExist()
    {
        // Arrange
        _repository.GetAllAsync(Arg.Any<Expression<Func<Domain.Bids.Bid, bool>>?>(), Arg.Any<Expression<Func<Domain.Bids.Bid, object>>[]?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok(new List<Altura.BidManagement.Domain.Bids.Bid>()));

        // Act
        var result = await _sut.Handle(new GetBidsQuery(), CancellationToken.None);

        // Assert
        result.Status.Should().BeFalse();
        result.Data.Should().BeEmpty();
        result.Message.Should().Be("No bids found.");
    }
} 