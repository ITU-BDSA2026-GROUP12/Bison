namespace Bison.Razor.Model;
using System;

public class Author
{
    String name{get; set;}
    String email{get; set;}


// this list is private, so it can only be accessed from inside the class
    private readonly List<Post> _posts = new();

    // this list is read-only, so it can be accessed from outside the class, but not modified
    public IReadOnlyList<Post> Posts => _posts;

    public Author(String name, String email)
    {
        this.name = name;
        this.email = email;
    }


// this is method where the post is added to the list of posts for the author
    internal void AddPost(Post post)
    {
        _posts.Add(post);
    }

}