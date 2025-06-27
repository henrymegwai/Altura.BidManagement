using MediatR;

namespace Altura.BidManagement.Application.Common.Interfaces
{
    public interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse> where TResponse : notnull;
}
