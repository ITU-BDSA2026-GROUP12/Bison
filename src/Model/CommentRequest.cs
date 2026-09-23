namespace Model
{
    public record CommentRequest(
        int ObservationId,
        string Author,
        string Message,
        long Timestamp
        );
}