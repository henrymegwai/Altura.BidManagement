using MediatR;

namespace Altura.BidManagement.Application.Common.Interfaces
{
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;
}
