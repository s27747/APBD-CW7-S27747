namespace MiniHelpdesk.Web.Services;

public class ServiceResult
{
    public bool Success { get; }
    public string? Error { get; }

    protected ServiceResult(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    public static ServiceResult Ok()
    {
        return new ServiceResult(true, null);
    }

    public static ServiceResult Fail(string error)
    {
        return new ServiceResult(false, error);
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; }

    private ServiceResult(bool success, string? error, T? data) : base(success, error)
    {
        Data = data;
    }

    public static ServiceResult<T> Ok(T data)
    {
        return new ServiceResult<T>(true, null, data);
    }

    public new static ServiceResult<T> Fail(string error)
    {
        return new ServiceResult<T>(false, error, default);
    }
}