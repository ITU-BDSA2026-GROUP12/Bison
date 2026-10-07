using Bison.Razor.Model;
using Microsoft.EntityFrameworkCore;

namespace Bison.Razor;

public class BisonDBContext : DbContext {
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options) {}

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Observation> Observations => Set<Observation>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Taxon> Taxons => Set<Taxon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
  
        // Observation, Comment and Proposal are stored as Posts.
        modelBuilder.Entity<Post>()
            .HasDiscriminator<string>("PostType")
            .HasValue<Observation>("Observation")
            .HasValue<Comment>("Comment")
            .HasValue<Proposal>("Proposal");

        // Author -> Posts
        modelBuilder.Entity<Post>()
            .HasOne(post => post.Author)
            .WithMany(author => author.Posts)
            .HasForeignKey(post => post.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Taxon parent -> children
        modelBuilder.Entity<Taxon>()
            .HasOne(taxon => taxon.Parent)
            .WithMany(taxon => taxon.Children)
            .HasForeignKey(taxon => taxon.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Observation -> Taxon
        modelBuilder.Entity<Observation>()
            .HasOne(observation => observation.Taxon)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        // Comment -> Observation
        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Observation)
            .WithMany(observation => observation.Comments)
            .HasForeignKey(comment => comment.ObservationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Proposal -> Observation
        modelBuilder.Entity<Proposal>()
            .HasOne(proposal => proposal.Observation)
            .WithMany(observation => observation.Proposals)
            .HasForeignKey(proposal => proposal.ObservationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Proposal -> Taxon
        modelBuilder.Entity<Proposal>()
            .HasOne(proposal => proposal.Taxon)
            .WithMany()
            .HasForeignKey(proposal => proposal.TaxonId)
            .OnDelete(DeleteBehavior.Restrict);

        // DwcTaxonId is the value the Entity Framework (EF) should store, as dwc_TaxonID is only a compatibility alias for DbInitializer.
        modelBuilder.Entity<Taxon>().Ignore(taxon => taxon.dwc_TaxonID);
    }
}