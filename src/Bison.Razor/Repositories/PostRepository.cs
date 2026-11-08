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
            observation.TimeStamp.ToString()
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
                observation.TimeStamp.ToString()
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
                observation.TimeStamp.ToString()
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
            .Include(proposal => proposal.Taxon)
            .Where(proposal => proposal.ObservationId == observationId)
            .OrderBy(proposal => proposal.PostId)
            .AsEnumerable()
            .ToList();
    }
    public int CreateObservation(string author, string message, DateTime timestamp) {
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



        
        
        return 67;


        
    }
}