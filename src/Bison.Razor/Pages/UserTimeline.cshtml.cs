using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;

namespace Bison.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel>? Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

        // this method gets called everytime a person sends a http get request
    // for now we just tell ASP.NET which page we are on. you do it by /?page=(page number)
    public ActionResult OnGet( string author, [FromQuery] int page = 1)
    {
        Observations = _service.GetObservationsFromAuthor(author,page);
        return Page();
    }
}
