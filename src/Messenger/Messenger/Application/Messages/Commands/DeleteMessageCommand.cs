﻿
using MassTransit;

using MediatR;

using YourBrand.Identity;
using YourBrand.Messenger.Contracts;
using YourBrand.Messenger.Domain;
using YourBrand.Messenger.Domain.Repositories;

namespace YourBrand.Messenger.Application.Messages.Commands;

public record DeleteMessageCommand(string ConversationId, string MessageId) : IRequest
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IBus _bus;

        public DeleteMessageCommandHandler(IConversationRepository conversationRepository, IMessageRepository messageRepository, IUnitOfWork unitOfWork, IUserContext userContext, IBus bus)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _bus = bus;
        }

        public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetConversation(request.ConversationId, cancellationToken);

            if (conversation is null) throw new Exception();

            var message = await _messageRepository.GetMessage(request.MessageId, cancellationToken);

            if (message is null) throw new Exception();

            if (!IsAuthorizedToDelete(message))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            conversation.DeleteMessage(message);

            //_messageRepository.DeleteMessage(message);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish(new MessageDeleted(null!, message.Id));

        }

        private bool IsAuthorizedToDelete(Domain.Entities.Message message) => _userContext.IsCurrentUser(message.CreatedById!) || _userContext.IsUserInRole(Roles.Administrator);
    }
}