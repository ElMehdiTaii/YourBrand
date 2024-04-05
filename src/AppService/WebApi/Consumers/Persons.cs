﻿using MassTransit;

using MediatR;

using YourBrand.Application.Users.Commands;
using YourBrand.Identity;
using YourBrand.IdentityManagement.Contracts;

namespace YourBrand.Consumers;

public class AppServiceUserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IMediator _mediator;
    private readonly IRequestClient<GetUser> _requestClient;
    private readonly ILogger<AppServiceUserCreatedConsumer> _logger;
    private readonly IUserContext _userContext;

    public AppServiceUserCreatedConsumer(IMediator mediator, IUserContext userContext, IRequestClient<GetUser> requestClient, ILogger<AppServiceUserCreatedConsumer> logger)
    {
        _mediator = mediator;
        _userContext = userContext;
        _requestClient = requestClient;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        try
        {
            var message = context.Message;

            _userContext.SetCurrentUser(message.CreatedById);

            var messageR = await _requestClient.GetResponse<GetUserResponse>(new GetUser(message.UserId, (message.CreatedById)));
            var message2 = messageR.Message;

            var result = await _mediator.Send(new CreateUserCommand(message2.UserId, message2.FirstName, message2.LastName, message2.DisplayName, "SSN", message2.Email));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "FOO");
        }
    }
}

public class AppServiceUserDeletedConsumer : IConsumer<UserDeleted>
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public AppServiceUserDeletedConsumer(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var message = context.Message;

        _userContext.SetCurrentUser(message.DeletedById);

        await _mediator.Send(new DeleteUserCommand(message.UserId));
    }
}

public class AppServiceUserUpdatedConsumer : IConsumer<UserUpdated>
{
    private readonly IMediator _mediator;
    private readonly IRequestClient<GetUser> _requestClient;
    private readonly IUserContext _userContext;

    public AppServiceUserUpdatedConsumer(IMediator mediator, IRequestClient<GetUser> requestClient, IUserContext userContext)
    {
        _mediator = mediator;
        _requestClient = requestClient;
        _userContext = userContext;
    }

    public async Task Consume(ConsumeContext<UserUpdated> context)
    {
        var message = context.Message;

        _userContext.SetCurrentUser(message.UpdatedById);

        var messageR = await _requestClient.GetResponse<GetUserResponse>(new GetUser(message.UserId, message.UpdatedById));
        var message2 = messageR.Message;

        var result = await _mediator.Send(new UpdateUserCommand(message2.UserId, message2.FirstName, message2.LastName, message2.DisplayName, "SSN", message2.Email));
    }
}