using MediatR;

namespace Altura.BidManagement.Application.Common.Interfaces
{
    public interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse> where TResponse: notnull
    {
    }
}
