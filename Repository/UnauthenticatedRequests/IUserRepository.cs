using aniList_cli.Repository.Models;

namespace aniList_cli.Repository.UnauthenticatedRequests;

public interface IUserRepository
{
   public Task<AniListUser?> GetUserById(int userId);


}