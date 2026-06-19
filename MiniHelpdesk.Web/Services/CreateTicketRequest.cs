using System.ComponentModel.DataAnnotations;

namespace MiniHelpdesk.Web.Services;

public class CreateTicketRequest
{
    [Required(ErrorMessage = "Tytuł jest wymagany.")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Autor jest wymagany.")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Treść pierwszego komentarza jest wymagana.")]
    public string FirstCommentContent { get; set; } = string.Empty;
}