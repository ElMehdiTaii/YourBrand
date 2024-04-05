using YourBrand.Identity;
using YourBrand.Ticketing.Application.Common;

namespace YourBrand.Ticketing.Application.Features.Tickets.EventHandlers;

public sealed class TicketStatusUpdatedEventHandler : IDomainEventHandler<TicketStatusUpdated>
{
    private readonly ITicketRepository ticketRepository;
    private readonly IUserContext userContext;
    private readonly IEmailService emailService;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketStatusUpdatedEventHandler(ITicketRepository ticketRepository, IUserContext userContext, IEmailService emailService, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.userContext = userContext;
        this.emailService = emailService;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketStatusUpdated notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await ticketNotificationService.StatusUpdated(ticket.Id, ticket.Status.ToDto());

        if (ticket.AssigneeId is not null && ticket.LastModifiedById != ticket.AssigneeId)
        {
            await emailService.SendEmail(ticket.Assignee!.Email,
                $"Status of \"{ticket.Subject}\" [{ticket.Id}] changed to {notification.NewStatus}.",
                $"{ticket.LastModifiedBy!.Name} changed status of \"{ticket.Subject}\" [{ticket.Id}] from {notification.OldStatus} to {notification.NewStatus}.");
        }
    }
}