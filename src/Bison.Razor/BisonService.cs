using Model;
using Bison.SQLite;

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations();
    public List<ObservationViewModel> GetObservationsFromAuthor(string author);
    public ObservationDetailsViewModel? GetObservationDetails(int observationId);
}

public class ObservationService : IObservationService
{
    private readonly DBFacade _dbFacade;

    public ObservationService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public List<ObservationViewModel> GetObservations()
    {
        return _dbFacade.GetObservations();
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        return _dbFacade.GetObservationsFromAuthor(author);
    }

    // Collects all information needed for the observation details page.
    public ObservationDetailsViewModel? GetObservationDetails(int observationId) {
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
