﻿using MediatR;

namespace Altura.BidManagement.Application.Common.Interfaces
{
    public interface ICommand<out TResponse> : IRequest<TResponse>;
}
