using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using aniList_cli.Repository.Models;
using aniList_cli.Service;
using aniList_cli.Settings;


namespace aniList_cli.Repository.AuthenticatedRequests;

public class MediaListRepository : IMediaListRepository
{
    private readonly AppParameter _parameter;

    private readonly ILoginService _loginService;

    public MediaListRepository(AppParameter parameter, ILoginService loginService)
    {
        _parameter = parameter;
        _loginService = loginService;
    }
    
    /// <summary>
    ///  searches MediaListCollection for logged in user. 
    /// </summary>
    /// <param name="type">ANIME or MANGA</param>
    /// <returns>MediaListCollection containing media lists of user. Returns null if no collection found. </returns>
    public async Task<MediaListCollection?> GetMediaListByUserId(MediaType type )
    {

        string userId = _loginService.GetUserId();
        string token = _loginService.GetToken();
        
        string graphQlQuery = "query UserListAnime {MediaListCollection(userId : " + userId + ", type : "+ type +"){lists {name,status,entries {id,status,progress,media{id,chapters,episodes,title {romaji,english,native,userPreferred}}}}}}";

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            
        HttpContent content = new StringContent("{ \"query\":\"" +graphQlQuery +"\"}", Encoding.UTF8);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        HttpResponseMessage response = await client.PostAsync(_parameter.ApiEndpoint, content);
        response.EnsureSuccessStatusCode();
        
        string responseJson = await response.Content.ReadAsStringAsync();
        string innerDoc = JsonDocument.Parse(responseJson).RootElement.GetProperty("data").ToString();
        MediaListCollectionResponse? result = JsonSerializer.Deserialize<MediaListCollectionResponse?>(innerDoc);
        
        if (result is { MediaListCollection.Lists.Count: > 0 })
        {
            return result.MediaListCollection;
        }

        return null;
    }
}