using Bison.Razor.Model;

using Microsoft.EntityFrameworkCore;

namespace Bison.Razor.Repositories;

public class PostRepository : IPostRepository {

    private readonly BisonDBContext _context;

    public PostRepository(BisonDBContext context) {
        _context = context;
    }

    public ObservationViewModel? GetObservation(int observationId) {
        var observation = _context.Observations
            .AsNoTracking()                                                         // Don't keep track for later updates.
            .Include(observation => observation.Author)                             // Also retrieve author.
            .FirstOrDefault(observation => observation.PostId == observationId);    // Safetly retrieve first observation.

        if (observation == null) {
            return null;
        }

        return new ObservationViewModel(
            observation.PostId,
            observation.Author.Name,
            observation.Text,
            observation.TimeStamp.ToString(),
            observation.PictureUrl
        );
    }

    public List<ObservationViewModel> GetObservations(int page) {
        page = Math.Max(page, 1);

        return _context.Observations
            .AsNoTracking()
            .Include(observation => observation.Author)
            .OrderBy(observation => observation.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(observation => new ObservationViewModel(                    // Fetch only the observations actually needed (32 max).
                observation.PostId,
                observation.Author.Name,
                observation.Text,
                observation.TimeStamp.ToString(),
                observation.PictureUrl
            ))
            .ToList();
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page) {
        page = Math.Max(page, 1);

        return _context.Observations
            .AsNoTracking()
            .Include(observation => observation.Author)
            .Where(observation => observation.Author.Name == author)
            .OrderBy(observation => observation.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(observation => new ObservationViewModel(
                observation.PostId,
                observation.Author.Name,
                observation.Text,
                observation.TimeStamp.ToString(),
                observation.PictureUrl
            ))
            .ToList();
    }

    public List<Comment> GetComments(int observationId) {
        return _context.Comments
            .AsNoTracking()
            .Include(comment => comment.Author)
            .Where(comment => comment.ObservationId == observationId)
            .OrderBy(comment => comment.PostId)
            .AsEnumerable()                                                     // Makes that time conversion happen in C#
            .ToList();
    }

    public List<Proposal> GetProposals(int observationId) {
        return _context.Proposals
            .AsNoTracking()
            .Include(proposal => proposal.Author)
            .Include(proposal => proposal.Taxon).ThenInclude(taxon => taxon.Parent)
            .Where(proposal => proposal.ObservationId == observationId)
            .OrderBy(proposal => proposal.PostId)
            .ToList();
    }
    public void CreateObservation(string author, string message, DateTime timestamp) {
        Model.Author authorEntity = null;

        // this loop basically just look for a existing author
        foreach (var aut in _context.Authors) {
            if(aut.Name == author) {
                authorEntity = aut;
                break;
            }
        }

        if(authorEntity == null) {
            authorEntity = new Model.Author{Name = author};

    
        }

        var observation = new Observation(authorEntity, message,timestamp, null);

        _context.Observations.Add(observation);
        _context.SaveChanges();
      
    }

    public void PostComment(string author, int observationId, string message, DateTime timestamp) {
        Model.Author authorEntity = null;

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
        var observation = _context.Observations.Find(observationId);
        if (observation == null) {
            throw new ArgumentException("Observation not found");
        }

        var comment = new Comment(authorEntity, message, timestamp, observationId);

        _context.Comments.Add(comment);
        _context.SaveChanges();
    }

    public void PostProposal(string author, int observationId, int taxonId, string message, DateTime timestamp) {
        Model.Author authorEntity = null;

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

        return query.OrderBy(observation => observation.PostId).ToList();
    }
}