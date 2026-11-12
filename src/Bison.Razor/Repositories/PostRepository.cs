using Bison.Razor.DTOs;
using Bison.Razor.Model;

using Microsoft.EntityFrameworkCore;

namespace Bison.Razor.Repositories;

public class PostRepository : IPostRepository {

    private readonly BisonDBContext _context;

    public PostRepository(BisonDBContext context) {
        _context = context;
    }

    public ObservationDTO? GetObservation(int observationId) {
        var observation = _context.Observations
            .AsNoTracking()                                                         // Don't keep track for later updates.
            .Include(observation => observation.Author)                             // Also retrieve author.
            .FirstOrDefault(observation => observation.PostId == observationId);    // Safetly retrieve first observation.

        if (observation == null) {
            return null;
        }

        return new ObservationDTO(
            observation.PostId,
            observation.Author.Name,
            observation.Text,
            observation.TimeStamp.ToString(),
            observation.PictureUrl
        );
    }

    public List<ObservationDTO> GetObservations(int page) {
        page = Math.Max(page, 1);

        return _context.Observations
            .AsNoTracking()
            .Include(observation => observation.Author)
            .OrderBy(observation => observation.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(observation => new ObservationDTO(                    // Fetch only the observations actually needed (32 max).
                observation.PostId,
                observation.Author.Name,
                observation.Text,
                observation.TimeStamp.ToString(),
                observation.PictureUrl
            ))
            .ToList();
    }

    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page) {
        page = Math.Max(page, 1);

        return _context.Observations
            .AsNoTracking()
            .Include(observation => observation.Author)
            .Where(observation => observation.Author.Name == author)
            .OrderBy(observation => observation.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(observation => new ObservationDTO(
                observation.PostId,
                observation.Author.Name,
                observation.Text,
                observation.TimeStamp.ToString(),
                observation.PictureUrl
            ))
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


    public void CreateObservation(string author, string message, DateTime timestamp) {
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

        var observation = new Observation(authorEntity, message, timestamp, null);

        _context.Observations.Add(observation);
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