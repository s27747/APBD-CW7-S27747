using Microsoft.EntityFrameworkCore;
using MiniHelpdesk.Web.Data;
using MiniHelpdesk.Web.Models;

namespace MiniHelpdesk.Web.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly HelpdeskDbContext _context;

    public TicketRepository(HelpdeskDbContext context)
    {
        _context = context;
    }

    public Task<List<Ticket>> GetAllAsync()
    {
        return _context.Tickets
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToListAsync();
    }

    public Task<Ticket?> GetByIdWithCommentsAsync(int id)
    {
        return _context.Tickets
            .Include(ticket => ticket.Comments.OrderBy(comment => comment.CreatedAt))
            .FirstOrDefaultAsync(ticket => ticket.Id == id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
    }
}