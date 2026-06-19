using MiniHelpdesk.Web.Models;

namespace MiniHelpdesk.Web.Repositories;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllAsync();
    Task<Ticket?> GetByIdWithCommentsAsync(int id);
    Task AddAsync(Ticket ticket);
}