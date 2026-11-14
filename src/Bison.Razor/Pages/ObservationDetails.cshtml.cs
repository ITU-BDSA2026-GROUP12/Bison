using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.Model;
using Bison.Razor.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Bison.Razor.Pages;

public class ObservationDetailsModel : PageModel
{
    private readonly IObservationService _service;

    public ObservationDetailsDTO? Observation { get; set; }

    public List<Taxon> Taxons { get; set; } = new();

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

        ObservationId = id.Value;

        // Get taxa for the dropdown.
        Taxons = _service.GetTaxons();

        return Page();
    }

    [BindProperty]
    public string Description { get; set; } = string.Empty;

    [BindProperty]
    public int ObservationId { get; set; }

    [BindProperty]
    public int SelectedTaxonId { get; set; }

    public ActionResult OnPostComment() {

        if (!ModelState.IsValid) {
            Observation = _service.GetObservationDetails(ObservationId);
            Taxons = _service.GetTaxons();

            return Page();
        }

        _service.PostComment(User.Identity!.Name!, ObservationId, Description);

        return RedirectToPage(new { id = ObservationId });
    }

    public ActionResult OnPostProposal() {

        //a bit messy way to get rid of Description from ModelState.
        //But it's better than making Description nullable in my opinion.
        ModelState.Remove(nameof(Description));
        if (!ModelState.IsValid) {

            Observation = _service.GetObservationDetails(ObservationId);
            Taxons = _service.GetTaxons();

            return Page();
        }

        _service.PostProposal(User.Identity!.Name!, ObservationId, SelectedTaxonId, string.Empty);

        return RedirectToPage(new { id = ObservationId });
    }

}