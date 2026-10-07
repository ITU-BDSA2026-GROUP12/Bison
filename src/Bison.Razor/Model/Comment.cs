namespace Bison.Razor.Model;
using System;

public class Comment : Post
{
    // Timestamp, author and text is already in post

    public int ObservationId { get; set; }

    public Observation Observation { get; set; } = null!;

    public Comment() {}
    
    // base is the super call to the Post constructor
    public Comment(Author author, String text, DateTime timestamp, int observationId) : base(author, text, timestamp)
    {
        ObservationId = observationId;
    }
}