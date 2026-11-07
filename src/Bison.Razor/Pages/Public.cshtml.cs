using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Bison.Razor.DTOs;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationDTO>? Observations { get; set; }
    public List<Bison.Razor.Model.Taxon> Taxons { get; set; } = new();
    public int? SelectedTaxonId { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }



    // this method gets called everytime a person sends a http get request
    // for now we just tell ASP.NET which page we are on. you do it by /?page=(page number)

    public ActionResult OnGet([FromQuery] int page = 1, [FromQuery] int? taxonId = null) {
        // Get taxa for the dropdown.
        Taxons = _service.GetTaxons();
        SelectedTaxonId = taxonId;

        // Use the existing timeline unless a taxon is selected.
        Observations = taxonId.HasValue
            ? _service.GetTimelineObservations(page, taxonId)
            : _service.GetObservations(page);

        return Page();
    }

}

