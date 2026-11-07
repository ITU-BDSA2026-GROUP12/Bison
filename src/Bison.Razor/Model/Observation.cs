namespace Bison.Razor.Model;
using System;

public class Observation : Post
{
    public Taxon? Taxon { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();

    public string? PictureUrl { get; set; }

    public Observation() {}
    
    public Observation(Author author, String text, DateTime timestamp, Taxon? taxon = null) : base(author, text, timestamp)
    {
        Taxon = taxon;
    }
}