using MiniHelpdesk.Web.Models;

namespace MiniHelpdesk.Web.Services;

public interface ITicketService
{
    Task<List<Ticket>> GetTicketsAsync();
    Task<Ticket?> GetTicketDetailsAsync(int id);
    Task<ServiceResult<int>> CreateTicketAsync(CreateTicketRequest request);
    Task<ServiceResult> CloseTicketAsync(int id);
}