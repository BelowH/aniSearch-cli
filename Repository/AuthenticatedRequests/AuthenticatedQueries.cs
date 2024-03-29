using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using aniList_cli.Repository.Models;
using aniList_cli.Service;
using aniList_cli.Settings;


namespace aniList_cli.Repository.AuthenticatedRequests;

public class AuthenticatedQueries : IAuthenticatedQueries
{
    private readonly AppParameter _parameter;
    private readonly ILoginService _loginService;

    public AuthenticatedQueries(AppParameter parameter, ILoginService loginService)
    {
        _parameter = parameter;
        _loginService = loginService;
    }

    /// <summary>
    ///  searches MediaListCollection for logged in user. 
    /// </summary>
    /// <param name="type">ANIME or MANGA</param>
    /// <param name="userId">the userId</param>
    /// <returns>MediaListCollection containing media lists of user. Returns null if no collection found. </returns>
    public MediaListCollection? GetMediaListByUserId(MediaType type,string userId )
    {
        string graphQlQuery = "query UserListAnime {MediaListCollection(userId : " + userId + ", type : "+ type +"){lists {name,status,entries {id,status,progress,media{id,chapters,episodes,type,title {romaji,english,native,userPreferred}}}}}}";

        MediaListCollection? result = QueryAuthenticatedRequest<MediaListCollectionResponse>(graphQlQuery).Result?.MediaListCollection;
        if (result is { Lists.Count: > 0 })
        {
            return result;
        }
        return null;
    }

    /// <summary>
    ///  checks if media is already in list of user.
    /// </summary>
    /// <param name="mediaId"></param>
    /// <returns>if it is in List returns a MediaStatusInfo else null</returns>
    public MediaStatusInfo? GetMediaStatusByMediaId(int mediaId)
    {
        string userId = _loginService.GetUserId();
        string graphQlQuery = "query MediaSearch {MediaList(userId: "+userId+" mediaId: "+mediaId+"){id, mediaId,status,progress,progressVolumes}}";

        try
        {
            return QueryAuthenticatedRequest<MediaStatusResponse>(graphQlQuery).Result?.MediaStatusInfo;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    ///  adds media to a List, or moves it to a new list.
    /// </summary>
    /// <param name="status">status e.g. list </param>
    /// <param name="mediaId">media id</param>
    /// <param name="currentMediaListId">id of the current mediaList item</param>
    public void AddMediaToList(MediaListStatus status, int mediaId, int? currentMediaListId = null)
    {
        string graphQlQuery;
        if (currentMediaListId != null)
        {
            graphQlQuery = "mutation MoveMedia {SaveMediaListEntry(id: "+currentMediaListId+", mediaId: "+mediaId+", status: "+status+") {id}}";
        }
        else
        {
            graphQlQuery = "mutation AddMedia{SaveMediaListEntry(mediaId: "+mediaId+" status: "+status+"){id}}";
        }
        _ = QueryAuthenticatedRequest<object?>(graphQlQuery).Result;
    }

    /// <summary>
    /// sets the progress of (episodes/chapters) of a media Item
    /// </summary>
    /// <param name="mediaId">media id</param>
    /// <param name="currentMediaListId">id of media in the current list</param>
    /// <param name="progress">amount of episodes/chapters</param>
    public void SetProgress(int mediaId, int currentMediaListId, int progress)
    {
        string graphQlQuery = "mutation AddMedia{SaveMediaListEntry(id: " + currentMediaListId + " mediaId: " + mediaId + " progress: "+progress+" ){id}}";
        _ = QueryAuthenticatedRequest<object?>(graphQlQuery).Result;
    }

    /// <summary>
    /// sets he progress of Volumes (only MANGA)
    /// </summary>
    /// <param name="mediaId">media id</param>
    /// <param name="currentMediaListId">if of media in the current list</param>
    /// <param name="volumeProgress">amount of volumes</param>
    public void SetVolumeProgress(int mediaId, int currentMediaListId, int volumeProgress)
    {
        string graphQlQuery = "mutation AddMedia{SaveMediaListEntry(id: " + currentMediaListId + " mediaId: " + mediaId + " progressVolumes: " + volumeProgress + " ){id}}";
        _ = QueryAuthenticatedRequest<object?>(graphQlQuery).Result;
    }
    
    /// <summary>
    ///  query the AniList api with a graphQlQuery
    /// </summary>
    /// <param name="graphQlQuery">the query as a string (will be encapsulated into a json http request)</param>
    /// <typeparam name="T">Return type</typeparam>
    /// <returns>parsed http response</returns>
    private async Task<T?> QueryAuthenticatedRequest<T>(string graphQlQuery)
    {
        string token = _loginService.GetToken();
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            
        HttpContent content = new StringContent("{ \"query\":\"" +graphQlQuery +"\"}", Encoding.UTF8);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        HttpResponseMessage response = await client.PostAsync(_parameter.ApiEndpoint, content);
        response.EnsureSuccessStatusCode();
        
        string responseJson = await response.Content.ReadAsStringAsync();
        string innerDoc = JsonDocument.Parse(responseJson).RootElement.GetProperty("data").ToString();
        T? result = JsonSerializer.Deserialize<T?>(innerDoc);
        
        return result;
    }
    
}