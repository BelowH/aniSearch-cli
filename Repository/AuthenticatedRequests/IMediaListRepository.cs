using aniList_cli.Repository.Models;

namespace aniList_cli.Repository.AuthenticatedRequests;

public interface IMediaListRepository
{
    public Task<MediaListCollection?>? GetMediaListByUserId(MediaType type);
}