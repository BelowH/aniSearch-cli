using aniList_cli.Repository.Models;

namespace aniList_cli.Repository.UnauthenticatedRequests;

public interface ISearchRepository
{
    public Task<Media> SearchById(int id);

    public Task<Page> SearchBySearchString(string searchQuery, int page, int perPage);
}