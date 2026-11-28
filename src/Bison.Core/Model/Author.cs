namespace Bison.Razor.Model;
using System;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Post> Posts { get; set; } = new();

    public Author() {}

    public Author(int authorId, String name, String email, List<Post> posts)
    {
        AuthorId = authorId;
        Name = name;
        Email = email;
        Posts = posts;
    }

    // this is method where the post is added to the list of posts for the author
    internal void AddPost(Post post)
    {
        Posts.Add(post);
    }

}