
using Bison.Core.Model;
using Microsoft.EntityFrameworkCore;
using Bison.Infastructure.DTOs;
using Bison.Infastructure.Database;

namespace Bison.Infastructure.Repositories;

public class PostRepository : IPostRepository {

    private readonly BisonDBContext _context;

    public PostRepository(BisonDBContext context) {
        _context = context;
    }

    private static ObservationDTO ToObservationDTO(Observation observation) {
        return new ObservationDTO(
            observation.PostId,
            observation.Author.Name,
            observation.Text,
            observation.TimeStamp.ToString(),
            observation.PictureUrl,
            observation.License,
            observation.IsPinned
        );
    }

    public ObservationDTO? GetObservation(int observationId) {
        var observation = _context.Observations
            .AsNoTracking()                                                         // Don't keep track for later updates.
            .Include(observation => observation.Author)                             // Also retrieve author.
            .FirstOrDefault(observation => observation.PostId == observationId);    // Safetly retrieve first observation.

        if (observation == null) {
            return null;
        }

        return ToObservationDTO(observation);
    }

    public List<ObservationDTO> GetObservations(int page) {
        page = Math.Max(page, 1);

        return _context.Observations
            .AsNoTracking()
            .Include(observation => observation.Author)
            //.OrderByDescending(observation => observation.IsPinned)
            //.ThenByDescending(observation => observation.PostId)
            .OrderBy(observation => observation.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .AsEnumerable()
            .Select(ToObservationDTO)
            .ToList();
    }

    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page) {
        page = Math.Max(page, 1);

        return _context.Observations
            .AsNoTracking()
            .Include(observation => observation.Author)
            .Where(observation => observation.Author.Name == author)
            .OrderByDescending(observation => observation.IsPinned)
            .ThenByDescending(observation => observation.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .AsEnumerable()
            .Select(ToObservationDTO)
            .ToList();
    }

    public List<CommentDTO> GetComments(int observationId) {
        return _context.Comments
            .AsNoTracking()
            .Include(comment => comment.Author)
            .Where(comment => comment.ObservationId == observationId)
            .OrderBy(comment => comment.PostId)
            .AsEnumerable()                                                     // Makes that time conversion happen in C#

            .Select(comment => new CommentDTO(
                comment.Author.Name,
                comment.Text,
                comment.TimeStamp.ToString()
            ))
            .ToList();
    }

    public List<ProposalDTO> GetProposals(int observationId) {
        return _context.Proposals
            .AsNoTracking()
            .Include(proposal => proposal.Author)
            .Include(proposal => proposal.Taxon).ThenInclude(taxon => taxon.Parent)
            .Where(proposal => proposal.ObservationId == observationId)
            .OrderBy(proposal => proposal.PostId)
            .AsEnumerable()
            .Select(proposal => new ProposalDTO(
            proposal.Author.Name,
            proposal.Taxon.DwcTaxonId,
            proposal.TimeStamp.ToString()
            ))
            .ToList();
    }

    public void CreateObservation(string author, string message, DateTime timestamp, string License) {
        Author? authorEntity = null;


        // this loop basically just look for a existing author
        foreach (var aut in _context.Authors) {
            if (aut.Name == author) {
                authorEntity = aut;
                break;
            }
        }

        if (authorEntity == null) {
            authorEntity = new Author { Name = author };


        }

        var observation = new Observation(authorEntity, message, timestamp, null, License);

        _context.Observations.Add(observation);
        _context.SaveChanges();

    }

    public void PostComment(string author, int observationId, string message, DateTime timestamp) {
        Author? authorEntity = null;

        // this loop basically just look for a existing author
        foreach (var aut in _context.Authors) {
            if (aut.Name == author) {
                authorEntity = aut;
                break;
            }
        }

        if (authorEntity == null) {
            authorEntity = new Author { Name = author };
        }

        //check if observation exists
        var observation = _context.Observations.Find(observationId);
        if (observation == null) {
            throw new ArgumentException("Observation not found");
        }

        var comment = new Comment(authorEntity, message, timestamp, observationId);

        _context.Comments.Add(comment);
        _context.SaveChanges();
    }

    public void PostProposal(string author, int observationId, int taxonId, string message, DateTime timestamp) {
        Model.Author? authorEntity = null;

        // this loop basically just look for a existing author
        foreach (var aut in _context.Authors) {
            if (aut.Name == author) {
                authorEntity = aut;
                break;
            }
        }

        if (authorEntity == null) {
            authorEntity = new Model.Author { Name = author };
        }

        //check if observation exists
        Observation? observation = _context.Observations.Find(observationId);
        if (observation == null) {
            throw new ArgumentException("Observation not found");
        }

        //check if taxon exists
        Taxon? taxon = _context.Taxons.Find(taxonId);
        if (taxon == null) {
            throw new ArgumentException("Taxon not found");
        }

        var proposal = new Proposal(authorEntity, message, timestamp, observationId, taxon);

        _context.Proposals.Add(proposal);
        _context.SaveChanges();
    }

    public void TogglePinnedObservation(string author, int observationId) {
        var observation = _context.Observations
            .Include(o => o.Author)
            .FirstOrDefault(o => o.PostId == observationId);

        if (observation == null)
        {
            return;
        }

        if (observation.Author.Name != author)
        {
            return;
        }

        observation.IsPinned = !observation.IsPinned;

        _context.SaveChanges();
    }

    public List<Model.Taxon> GetTaxons() {
        return _context.Taxons
            .AsNoTracking()
            .OrderBy(taxon => taxon.TaxonId)
            .ToList();

    }

    public List<Model.Observation>
        GetObservationsForFiltering(string? author = null) {
        // Load all taxa so EF Core can connect each taxon with its ancestors.
        _context.Taxons.Load();

        var query = _context.Observations
            .Include(observation => observation.Author)
            .Include(observation => observation.Taxon)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(author)) {
            query = query.Where(observation => observation.Author.Name == author);
        }

        return query
            .OrderByDescending(observation => observation.IsPinned)
            .ThenByDescending(observation => observation.PostId)
            .ToList();
    }

    public int CountObservations(string? author = null) {
        var query = _context.Observations.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(author)) {
            query = query.Where(o => o.Author.Name == author);
        }

        return query.Count();
    }

    public AuthorDTO? GetAuthor(string authorName) {
        var author = _context.Authors
            .AsNoTracking()
            .FirstOrDefault(author => author.Name == authorName);

        if (author == null) {
            return null;
        }

        return new AuthorDTO(
            author.AuthorId,
            author.Name,
            author.Email
        );
    }

    public List<CommentDTO>? GetCommentsByAuthor(string authorName) {
        return _context.Comments
            .AsNoTracking()
            .Include(comment => comment.Author)
            .Where(comment => comment.Author.Name == authorName)
            .OrderBy(comment => comment.PostId)
            .AsEnumerable()

            .Select(comment => new CommentDTO(
                comment.Author.Name,
                comment.Text,
                comment.TimeStamp.ToString()
            ))
            .ToList();
    }

    public List<ProposalDTO>? GetProposalsByAuthor(string authorName) {
        return _context.Proposals
            .AsNoTracking()
            .Include(proposal => proposal.Author)
            .Include(proposal => proposal.Taxon).ThenInclude(taxon => taxon.Parent)
            .Where(proposal => proposal.Author.Name == authorName)
            .OrderBy(proposal => proposal.PostId)
            .AsEnumerable()
            .Select(proposal => new ProposalDTO(
            proposal.Author.Name,
            //proposal.Taxon.DwcTaxonId,
            proposal.Taxon.DwcTaxonId,
            proposal.TimeStamp.ToString()
            ))
            .ToList();
    }
}