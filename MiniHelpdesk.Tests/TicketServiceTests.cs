using MiniHelpdesk.Web.Models;
using MiniHelpdesk.Web.Repositories;
using MiniHelpdesk.Web.Services;

namespace MiniHelpdesk.Tests;

public class TicketServiceTests
{
    [Fact]
    public async Task CreateTicketAsync_WithValidData_CreatesTicketWithFirstComment()
    {
        var ticketRepository = new FakeTicketRepository();
        var commentRepository = new FakeTicketCommentRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new TicketService(ticketRepository, commentRepository, unitOfWork);

        var request = new CreateTicketRequest
        {
            Title = "Problem z logowaniem",
            Description = "Nie mogę zalogować się do systemu",
            Author = "Jakub",
            FirstCommentContent = "Problem występuje od rana"
        };

        var result = await service.CreateTicketAsync(request);

        Assert.True(result.Success);
        Assert.Single(ticketRepository.Tickets);
        Assert.Single(commentRepository.Comments);
        Assert.Equal("Problem z logowaniem", ticketRepository.Tickets[0].Title);
        Assert.Equal("Problem występuje od rana", commentRepository.Comments[0].Content);
        Assert.True(unitOfWork.WasTransactionStarted);
        Assert.True(unitOfWork.WasSaved);
        Assert.True(unitOfWork.WasCommitted);
    }

    [Fact]
    public async Task CreateTicketAsync_WithEmptyTitle_ReturnsValidationError()
    {
        var ticketRepository = new FakeTicketRepository();
        var commentRepository = new FakeTicketCommentRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new TicketService(ticketRepository, commentRepository, unitOfWork);

        var request = new CreateTicketRequest
        {
            Title = "",
            Description = "Opis",
            Author = "Jakub",
            FirstCommentContent = "Komentarz"
        };

        var result = await service.CreateTicketAsync(request);

        Assert.False(result.Success);
        Assert.Empty(ticketRepository.Tickets);
        Assert.Empty(commentRepository.Comments);
        Assert.False(unitOfWork.WasTransactionStarted);
        Assert.False(unitOfWork.WasSaved);
        Assert.False(unitOfWork.WasCommitted);
    }

    [Fact]
    public async Task CloseTicketAsync_WithExistingTicket_ChangesStatusToClosed()
    {
        var ticketRepository = new FakeTicketRepository();
        var commentRepository = new FakeTicketCommentRepository();
        var unitOfWork = new FakeUnitOfWork();

        ticketRepository.Tickets.Add(new Ticket
        {
            Id = 1,
            Title = "Awaria drukarki",
            Description = "Drukarka nie działa",
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        });

        var service = new TicketService(ticketRepository, commentRepository, unitOfWork);

        var result = await service.CloseTicketAsync(1);

        Assert.True(result.Success);
        Assert.Equal(TicketStatus.Closed, ticketRepository.Tickets[0].Status);
        Assert.True(unitOfWork.WasSaved);
    }

    [Fact]
    public async Task CreateTicketAsync_WhenCommentSaveFails_DoesNotCommitTransaction()
    {
        var ticketRepository = new FakeTicketRepository();
        var commentRepository = new FakeTicketCommentRepository(true);
        var unitOfWork = new FakeUnitOfWork();

        var service = new TicketService(ticketRepository, commentRepository, unitOfWork);

        var request = new CreateTicketRequest
        {
            Title = "Awaria",
            Description = "Opis awarii",
            Author = "Jakub",
            FirstCommentContent = "Komentarz"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateTicketAsync(request));

        Assert.True(unitOfWork.WasTransactionStarted);
        Assert.False(unitOfWork.WasCommitted);
    }

    private class FakeTicketRepository : ITicketRepository
    {
        public List<Ticket> Tickets { get; } = new();

        public Task<List<Ticket>> GetAllAsync()
        {
            return Task.FromResult(Tickets);
        }

        public Task<Ticket?> GetByIdWithCommentsAsync(int id)
        {
            return Task.FromResult(Tickets.FirstOrDefault(ticket => ticket.Id == id));
        }

        public Task AddAsync(Ticket ticket)
        {
            ticket.Id = Tickets.Count + 1;
            Tickets.Add(ticket);
            return Task.CompletedTask;
        }
    }

    private class FakeTicketCommentRepository : ITicketCommentRepository
    {
        private readonly bool _shouldFail;

        public FakeTicketCommentRepository(bool shouldFail = false)
        {
            _shouldFail = shouldFail;
        }

        public List<TicketComment> Comments { get; } = new();

        public Task AddAsync(TicketComment comment)
        {
            if (_shouldFail)
            {
                throw new InvalidOperationException("Comment save failed");
            }

            Comments.Add(comment);
            return Task.CompletedTask;
        }
    }

    private class FakeUnitOfWork : IUnitOfWork
    {
        public bool WasTransactionStarted { get; private set; }
        public bool WasCommitted { get; private set; }
        public bool WasSaved { get; private set; }

        public Task<IAppTransaction> BeginTransactionAsync()
        {
            WasTransactionStarted = true;
            return Task.FromResult<IAppTransaction>(new FakeTransaction(this));
        }

        public Task SaveChangesAsync()
        {
            WasSaved = true;
            return Task.CompletedTask;
        }

        private class FakeTransaction : IAppTransaction
        {
            private readonly FakeUnitOfWork _unitOfWork;

            public FakeTransaction(FakeUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public Task CommitAsync()
            {
                _unitOfWork.WasCommitted = true;
                return Task.CompletedTask;
            }

            public ValueTask DisposeAsync()
            {
                return ValueTask.CompletedTask;
            }
        }
    }
}