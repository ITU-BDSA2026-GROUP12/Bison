namespace Bison.Razor.Model;
using System;

public class Comment : Post
{
    // Timestamp, author and text is already in post

    int observationId;

    public Observation observation{get;}

    // base is the super call to the Post constructor
    public Comment(Author author, String text, DateTime timestamp, int observationId) : base(author, text, timestamp)
    {
        this.observationId = observationId;
    }
}