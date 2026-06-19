namespace MiniHelpdesk.Web.Repositories;

public interface IAppTransaction : IAsyncDisposable
{
    Task CommitAsync();
}