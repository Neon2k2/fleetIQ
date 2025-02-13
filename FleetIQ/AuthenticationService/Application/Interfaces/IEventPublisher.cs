using System;

namespace AuthenticationService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class;
}

