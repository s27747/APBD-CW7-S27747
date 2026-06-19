using MiniHelpdesk.Web.Models;

namespace MiniHelpdesk.Web.Repositories;

public interface ITicketCommentRepository
{
    Task AddAsync(TicketComment comment);
}