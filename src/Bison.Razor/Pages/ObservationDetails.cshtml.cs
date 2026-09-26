using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;

namespace Bison.Razor.Pages;

public class ObservationDetailsModel : PageModel {
    private readonly IObservationService _service;

    public ObservationDetailsViewModel? Observation { get; set; }

    public ObservationDetailsModel(IObservationService service) {
        _service = service;
    }

    public IActionResult OnGet(int? id) {
        if (id == null) {
            return Redirect("/");
        }

        Observation = _service.GetObservationDetails(id.Value);

        if (Observation == null) {
            return NotFound();
        }

        return Page();
    }
}