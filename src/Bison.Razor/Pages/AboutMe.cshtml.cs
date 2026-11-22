using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.DTOs;

namespace Bison.Razor.Pages;

public class AboutMe : PageModel {
    private readonly IObservationService _service;
    public AuthorDTO? Author { get; set; }
    public List<ObservationDTO>? Observations { get; set; }
    public List<CommentDTO>? Comments { get; set; }

    public AboutMe(IObservationService service) {
        _service = service;
    }

    public ActionResult OnGet(int page = 1) {
        Author = _service.GetAuthor(User.Identity!.Name!);
        Observations = _service.GetObservationsFromAuthor(User.Identity!.Name!, page);
        Comments = _service.GetCommentsByAuthor(User.Identity!.Name!);
        return Page();
    }
}
