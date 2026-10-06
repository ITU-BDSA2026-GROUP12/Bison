namespace Bison.Razor.Model;
using System;


public abstract class Post
{
    public int PostId { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime TimeStamp { get; set; }

    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    protected Post() {}
    
    protected Post(Author author, String text, DateTime timestamp)
    {
        Text = text;
        Author = author;
        TimeStamp = timestamp;

        // here we add the post to the authors list of posts, so that the author can see all the posts they have made
        author.AddPost(this);

    }
}
