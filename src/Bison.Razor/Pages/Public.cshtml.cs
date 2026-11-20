using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.Model;
using Bison.Razor.DTOs;
using Bison.Razor.Repositories;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel {
    private readonly IObservationService _service;
    public List<ObservationDTO>? Observations { get; set; }
    public List<Model.Taxon> Taxons { get; set; } = new();
    public int? SelectedTaxonId { get; set; }
    public int CurrentPage { get; set; } = 1;
    public bool HasNextPage { get; set; }

    public PublicModel(IObservationService service) {
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

        const int pageSize = 32;

        int observationCount = _service.GetTimelineObservationCount(taxonId);

        TotalPages = Math.Max(1, (int)Math.Ceiling(observationCount / (double)pageSize));

        // Keep the current page within valid bounds.
        CurrentPage = Math.Clamp(page, 1, TotalPages);

        // Retrieve observations for the current page.
        Observations = _service.GetTimelineObservations(
            CurrentPage,
            taxonId
        );

        // Check whether another page exists.
        HasNextPage = CurrentPage < TotalPages;

        return Page();
    }


    // so this method is used to post.

    [BindProperty]

    public string description { get; set; } = string.Empty;

    public ActionResult OnPost() {

        if (!ModelState.IsValid) {
            Observations = _service.GetObservations(1);
            return Page();
        }

        _service.CreateObervation(User.Identity!.Name!, description, License);
        return RedirectToPage("Public");
    }
    public int TotalPages { get; set; } = 1;
    public IEnumerable<int?> VisiblePages {
        get {
            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, CurrentPage + 2);


            // Always show the first page.
            yield return 1;

            // Show an ellipsis (...) if pages are skipped.
            if (start > 2) {
                yield return null;
            }

            // Show the pages around the current page.
            for (int page = Math.Max(2, start); page <= Math.Min(TotalPages - 1, end); page++) {
                yield return page;
            }

            // Show an ellipsis (...) before the last page.
            if (end < TotalPages - 1) {
                yield return null;
            }

            // Always show the last page.
            if (TotalPages > 1) {
                yield return TotalPages;
            }
        }
    }

    [BindProperty]
    public string? License { get; set; }

}



