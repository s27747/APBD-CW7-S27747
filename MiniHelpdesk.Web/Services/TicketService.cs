using MiniHelpdesk.Web.Models;
using MiniHelpdesk.Web.Repositories;

namespace MiniHelpdesk.Web.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCommentRepository _ticketCommentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(
        ITicketRepository ticketRepository,
        ITicketCommentRepository ticketCommentRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _ticketCommentRepository = ticketCommentRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<List<Ticket>> GetTicketsAsync()
    {
        return _ticketRepository.GetAllAsync();
    }

    public Task<Ticket?> GetTicketDetailsAsync(int id)
    {
        return _ticketRepository.GetByIdWithCommentsAsync(id);
    }

    public async Task<ServiceResult<int>> CreateTicketAsync(CreateTicketRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return ServiceResult<int>.Fail("Tytuł zgłoszenia jest wymagany.");
        }

        if (string.IsNullOrWhiteSpace(request.FirstCommentContent))
        {
            return ServiceResult<int>.Fail("Pierwszy komentarz jest wymagany.");
        }

        if (string.IsNullOrWhiteSpace(request.Author))
        {
            return ServiceResult<int>.Fail("Autor komentarza jest wymagany.");
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        var comment = new TicketComment
        {
            Ticket = ticket,
            Author = request.Author.Trim(),
            Content = request.FirstCommentContent.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _ticketRepository.AddAsync(ticket);
        await _ticketCommentRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        await transaction.CommitAsync();

        return ServiceResult<int>.Ok(ticket.Id);
    }

    public async Task<ServiceResult> CloseTicketAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdWithCommentsAsync(id);

        if (ticket is null)
        {
            return ServiceResult.Fail("Nie znaleziono zgłoszenia.");
        }

        ticket.Status = TicketStatus.Closed;

        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Ok();
    }
}