namespace Model
{
    public record ObservationViewModel(
        int ObservationId,
        string Author,
        string Message,
        string Timestamp
    );
}