namespace Bison.Razor.Model;
using System;

public class Observation : Post
{
    int observationId;
    public Taxon taxon{get; set;}
    public Observation(Author author, String text, DateTime timestamp, Taxon taxon = null) : base(author, text, timestamp)
    {
        this.taxon = taxon;
    }




}