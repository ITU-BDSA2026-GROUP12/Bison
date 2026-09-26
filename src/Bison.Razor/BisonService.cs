using Model;
using Bison.SQLite;

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    public ObservationDetailsViewModel? GetObservationDetails(int observationId, int page);
}

public class ObservationService : IObservationService
{
    private readonly DBFacade _dbFacade;

    public ObservationService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        return _dbFacade.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        return _dbFacade.GetObservationsFromAuthor(author, page);
    }

    // Collects all information needed for the observation details page.
    public ObservationDetailsViewModel? GetObservationDetails(int observationId, int page) {
        var observation = _dbFacade.GetObservation(observationId);

        if (observation == null) {
            return null;
        }

        var comments = _dbFacade.GetComments(observationId);

        var proposals = _dbFacade.GetProposals(observationId);

        return new ObservationDetailsViewModel(
            observationId,
            observation.Author,
            observation.Message,
            observation.Timestamp,
            comments,
            proposals
        );
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
