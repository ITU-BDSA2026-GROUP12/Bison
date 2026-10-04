namespace Bison.Razor.Model;
using System;


public abstract class Post
{
    public String text{get; set;}
    public Author author{get; set;}
    public DateTime timestamp{get;}

protected Post(Author author, String text, DateTime timestamp)
    {
        this.text = text;
        this.author = author;
        this.timestamp = timestamp;


        // here we add the post to the authors list of posts, so that the author can see all the posts they have made
        author.AddPost(this);

    }
    

}
