using Microsoft.EntityFrameworkCore.Storage;

namespace MiniHelpdesk.Web.Repositories;

public class EfAppTransaction : IAppTransaction
{
    private readonly IDbContextTransaction _transaction;

    public EfAppTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync()
    {
        return _transaction.CommitAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _transaction.DisposeAsync();
    }
}