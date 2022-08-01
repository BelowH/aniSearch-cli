using aniList_cli.Repository.Models;

namespace aniList_cli.Repository.AuthenticatedRequests;

public interface IAuthenticatedQueries
{
    public MediaListCollection? GetMediaListByUserId(MediaType type);

    public MediaStatusInfo? GetMediaStatusByMediaId(int mediaId);

}