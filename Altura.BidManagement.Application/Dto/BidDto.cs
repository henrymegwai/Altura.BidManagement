using Altura.BidManagement.Application.Common;

namespace Altura.BidManagement.Application.Dto;

public record BidDto(
    Guid Id, 
    string Title,
    decimal Amount,
    string State, 
    DateTime CreatedAt);