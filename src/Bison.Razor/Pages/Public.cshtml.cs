using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.Model;
using Bison.Razor.DTOs;
using Bison.Razor.Repositories;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationDTO>? Observations { get; set; }
    public List<Model.Taxon> Taxons { get; set; } = new();
    public int? SelectedTaxonId { get; set; }
    public int CurrentPage { get; set; } = 1;
    public bool HasNextPage { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }



    // this method gets called everytime a person sends a http get request
    // for now we just tell ASP.NET which page we are on. you do it by /?page=(page number)

    public ActionResult OnGet([FromQuery] int page = 1, [FromQuery] int? taxonId = null) {
        // Keep the current page within valid bounds.
        CurrentPage = Math.Max(1, page);
        
        // Get taxa for the dropdown.
        Taxons = _service.GetTaxons();
        SelectedTaxonId = taxonId;

        // Retrieve observations for the current page.
        Observations = _service.GetTimelineObservations(
            CurrentPage,
            taxonId
        );

        // Check whether another page exists.
        HasNextPage = _service.GetTimelineObservations(
            CurrentPage + 1,
            taxonId
        ).Any();

        return Page();
    }


    // so this method is used to post.

    [BindProperty]

    public string description{get; set;}
    public ActionResult OnPost() {

        if (!ModelState.IsValid) {
            Observations = _service.GetObservations(1);
            return Page();
        }

        _service.CreateObervation(User.Identity!.Name!, description);
            return RedirectToPage("Public");
    }

}

