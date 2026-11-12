using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Bison.Razor.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Bison.Razor.Pages;

public class ObservationDetailsModel : PageModel
{
    private readonly IObservationService _service;

    public ObservationDetailsDTO? Observation { get; set; }

    public ObservationDetailsModel(IObservationService service)
    {
        _service = service;
    }

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return Redirect("/");
        }

        Observation = _service.GetObservationDetails(id.Value);

        if (Observation == null)
        {
            return NotFound();
        }

        observationId = id.Value;

        return Page();
    }

    // so this method is used to post.
    [BindProperty]
    public string description { get; set; }

    [BindProperty]
    public int observationId { get; set; }

    public ActionResult OnPost() {

        if (!ModelState.IsValid) {
            Observation = _service.GetObservationDetails(observationId);
            return Page();
        }

        _service.PostComment(User.Identity!.Name!, observationId, description);

        return RedirectToPage(new { id = observationId });
    }


}