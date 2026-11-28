namespace Bison.Razor.Model;
using System;


public class Proposal : Post
{
    // Timestamp, author and text is already in post
    public int ObservationId { get; set; }

    public Observation Observation { get; set; } = null!;

    public int TaxonId { get; set; }

    public Taxon Taxon { get; set; } = null!;

    public Proposal() {}
    
    public Proposal(Author author, String text, DateTime timestamp, int observationId, Taxon taxon) : base(author, text, timestamp)
    {
        ObservationId = observationId;
        Taxon = taxon;
    }
}