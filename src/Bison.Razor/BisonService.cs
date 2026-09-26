using Model;
using Bison.SQLite;

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
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

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
