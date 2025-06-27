using FluentResults;

namespace Altura.BidManagement.Application.Common.Models;

public record Response<T>(bool Status, T Data, string Message, Result Error = null!);