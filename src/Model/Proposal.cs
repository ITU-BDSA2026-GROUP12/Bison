namespace Model
{
    public record Proposal(
        int ObservationId,
        string Author,
        string TaxonId,
        long Timestamp
        );
}