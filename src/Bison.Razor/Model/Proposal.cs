namespace Bison.Razor.Model;
using System;


public class Proposal : Post
{
    // Timestamp, author and text is already in post
    int observationId;
    public Observation? observation{get;}
    public Taxon taxon{get;}

    public Proposal(Author author, String text, DateTime timestamp, int observationId, Taxon taxon) : base(author, text, timestamp)
    {
        this.observationId = observationId;
        this.taxon = taxon;
    }
}