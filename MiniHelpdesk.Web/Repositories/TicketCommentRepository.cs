using MiniHelpdesk.Web.Data;
using MiniHelpdesk.Web.Models;

namespace MiniHelpdesk.Web.Repositories;

public class TicketCommentRepository : ITicketCommentRepository
{
    private readonly HelpdeskDbContext _context;

    public TicketCommentRepository(HelpdeskDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TicketComment comment)
    {
        await _context.TicketComments.AddAsync(comment);
    }
}