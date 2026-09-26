namespace Model;

public record ObservationDetailsViewModel(
    int ObservationId,
    string Author,
    string Message,
    string Timestamp,
    List<Comment> Comments,
    List<Proposal> Proposals
);

// The reason why I made a new class instead of updating ObservationViewModel is because ObservationViewModel
// focuses on the basic information for an observation, while ObservationDetailsViewModel focuses on the
// detailed view of a single observation, including its comments and proposals.