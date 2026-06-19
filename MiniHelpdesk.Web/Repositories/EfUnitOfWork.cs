using MiniHelpdesk.Web.Data;

namespace MiniHelpdesk.Web.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly HelpdeskDbContext _context;

    public EfUnitOfWork(HelpdeskDbContext context)
    {
        _context = context;
    }

    public async Task<IAppTransaction> BeginTransactionAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        return new EfAppTransaction(transaction);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}