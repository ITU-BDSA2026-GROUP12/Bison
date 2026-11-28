namespace Bison.Core.Model;

    public record ProposalRequest(
        int ObservationId,
        string Author,
        string TaxonId,
        long Timestamp
        );
