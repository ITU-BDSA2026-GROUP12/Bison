using Model;
using Bison.Razor;
using Bison.Razor.Repositories;

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    public ObservationDetailsViewModel? GetObservationDetails(int observationId);
}

public class ObservationService : IObservationService
{
    private readonly IPostRepository _repository;

    public ObservationService(IPostRepository repository)
    {
        _repository = repository;
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        return _repository.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        return _repository.GetObservationsFromAuthor(author, page);
    }

    // Collects all information needed for the observation details page.
    public ObservationDetailsViewModel? GetObservationDetails(int observationId) {
        return _repository.GetObservationDetails(observationId);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
