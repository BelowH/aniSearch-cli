using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using aniList_cli.Repository.Models;
using aniList_cli.Settings;


namespace aniList_cli.Repository.UnauthenticatedRequests;

public class UnAuthenticatedQueries :  IUnAuthenticatedQueries
{
    private readonly AppParameter _parameter;
    
    public UnAuthenticatedQueries(AppParameter parameter)
    {
        _parameter = parameter;
    }
    
    /// <summary>
    ///  gets a Media with all details by id.
    ///  returns null it something goes wrong.
    /// </summary>
    /// <param name="id">media id</param>
    /// <returns>Media or null</returns>
    public Media? SearchById(int id)
    {
        string graphQlQuery = "query MediaSearch{Media(id: "+id+"){id,title{romaji,english,native}type,format,description,season,seasonYear,episodes,averageScore,meanScore,genres,studios {nodes {name,isAnimationStudio}}startDate {year,month,day}endDate {year,month,day}status,chapters,volumes}}";
        return QueryApi<MediaData?>(graphQlQuery).Result?.Media;
    }

    /// <summary>
    /// searches a Medias by string and returns a page or null if something goes wrong.
    /// </summary>
    /// <param name="searchQuery">the search string</param>
    /// <param name="page">page number to query</param>
    /// <param name="perPage">results per page</param>
    /// <returns>Page with results or null</returns>
    public Page? SearchBySearchString(string searchQuery, int page, int perPage)
    {
        string graphQlQuery = 
            "query PageSearch{Page(page: "+page+" perPage: "+perPage+"){pageInfo{perPage,currentPage,lastPage,hasNextPage}media(search: \\\""+searchQuery+"\\\"){id,type,title{romaji,english,native}}}}";
        return  QueryApi<PageData?>(graphQlQuery).Result?.Page;
    }

    /// <summary>
    ///  queries api for aniList user by userId
    /// </summary>
    /// <param name="userId">aniList userId (jwtToken subject)</param>
    /// <returns>aniList user object with statistics, name, about etc.</returns>
    public AniListUser? GetUserById(int userId)
    {
        string graphQlQuery =
            "query UserSearch{User(id: "+userId+") {name, about(asHtml: true),siteUrl, updatedAt,statistics {anime {count,meanScore,minutesWatched,episodesWatched,chaptersRead,volumesRead,statuses {count,meanScore,chaptersRead,minutesWatched,status}}manga {count,meanScore,minutesWatched,episodesWatched,chaptersRead,volumesRead,statuses {count,meanScore,chaptersRead,minutesWatched,status}}}}}";
        return QueryApi<AniListUserData?>(graphQlQuery).Result?.User;
    }

    private async Task<T?> QueryApi<T>(string graphQlQuery)
    {
        using HttpClient client = new HttpClient();
        HttpContent content = new StringContent("{ \"query\":\"" + graphQlQuery + "\"}", Encoding.UTF8);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        HttpResponseMessage response = await client.PostAsync(_parameter.ApiEndpoint, content);
        response.EnsureSuccessStatusCode();

        string responseJson = await response.Content.ReadAsStringAsync();
        string innerDoc = JsonDocument.Parse(responseJson).RootElement.GetProperty("data").ToString();
        T? result = JsonSerializer.Deserialize<T>(innerDoc);

        return result;
    }
    
}