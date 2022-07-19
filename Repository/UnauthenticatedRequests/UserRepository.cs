using aniList_cli.Repository.Models;
using aniList_cli.Settings;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace aniList_cli.Repository.UnauthenticatedRequests;

public class UserRepository : IUserRepository
{

    private readonly AppParameter _parameter;
    
    public UserRepository(AppParameter parameter)
    {
        _parameter = parameter;
    }
    
    /// <summary>
    ///  queries api for aniList user by userId
    /// </summary>
    /// <param name="userId">aniList userId (jwtToken subject)</param>
    /// <returns>aniList user object with statistics, name, about etc.</returns>
    public async Task<AniListUser?> GetUserById(int userId)
    {
        try
        {
            GraphQLRequest request = new GraphQLRequest()
            {
                Query = "query UserSearch($userId: Int) {" +
                        " User(id: $userId) {" +
                        "    name" +
                        "    about(asHtml: true)" +
                        "    siteUrl" +
                        "    updatedAt" +
                        "    statistics {" +
                        "      anime {" +
                        "        count" +
                        "        meanScore" +
                        "        minutesWatched" +
                        "        episodesWatched" +
                        "        chaptersRead" +
                        "        volumesRead" +
                        "        statuses {" +
                        "          count" +
                        "          meanScore" +
                        "          chaptersRead" +
                        "          minutesWatched" +
                        "          status" +
                        "        }" +
                        "      }" +
                        "      manga {" +
                        "        count" +
                        "        meanScore" +
                        "        minutesWatched" +
                        "        episodesWatched" +
                        "        chaptersRead" +
                        "        volumesRead" +
                        "        statuses {" +
                        "          count" +
                        "          meanScore" +
                        "          chaptersRead" +
                        "          minutesWatched" +
                        "          status" +
                        "        }" +
                        "      }" +
                        "    }" +
                        "  }" +
                        "}",
                OperationName = "UserSearch",
                Variables = new {userId = userId}
            };
            using (GraphQLHttpClient client = new GraphQLHttpClient(_parameter.ApiEndpoint!, new NewtonsoftJsonSerializer()))
            {
                GraphQLResponse<AniListUserData> response = await client.SendQueryAsync<AniListUserData>(request);
                return response.Data.User;
            }
        }
        catch (GraphQLHttpRequestException requestException)
        {
            //In case the request fails.
            Console.WriteLine("An error occurred while searching." +requestException.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while searching." +e);
            throw;
        }
    }
}