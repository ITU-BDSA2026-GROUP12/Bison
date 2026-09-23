namespace Model
{
    public record ObservationRequest(
        string Author,
        string Message,
        long Timestamp,
        string? Location = null
        );
}