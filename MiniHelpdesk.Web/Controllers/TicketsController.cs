using Microsoft.AspNetCore.Mvc;
using MiniHelpdesk.Web.Services;

namespace MiniHelpdesk.Web.Controllers;

public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _ticketService.GetTicketsAsync();
        return View(tickets);
    }

    public IActionResult Create()
    {
        return View(new CreateTicketRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTicketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _ticketService.CreateTicketAsync(request);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(request);
        }

        return RedirectToAction(nameof(Details), new { id = result.Data });
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _ticketService.GetTicketDetailsAsync(id);

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        var result = await _ticketService.CloseTicketAsync(id);

        if (!result.Success)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}