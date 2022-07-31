using aniList_cli.Repository.Models;

namespace aniList_cli.Repository.UnauthenticatedRequests;

public interface IUnAuthenticatedQueries
{
    public Media? SearchById(int id);

    public Page? SearchBySearchString(string searchQuery, int page, int perPage);
    
    public AniListUser? GetUserById(int userId);
}