namespace Bison.Razor.Model;
using System;

public class Favorite
{
    public int FavoriteId { get; set; }

    public int PostId { get; set; }

    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    public Post Post { get; set; } = null!;

    //protected Favorite() {}

    protected Favorite(Author author, Post post)
    {
        Author = author;
        Post = post;

        // here we add the favorite to the authors list of favorites
        author.AddFavorite(this);
    }
}