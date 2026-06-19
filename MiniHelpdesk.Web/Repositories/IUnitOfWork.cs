namespace MiniHelpdesk.Web.Repositories;

public interface IUnitOfWork
{
    Task<IAppTransaction> BeginTransactionAsync();
    Task SaveChangesAsync();
}