using aniList_cli.Repository.Models;
using aniList_cli.Settings;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace aniList_cli.Repository.UnauthenticatedRequests;

public class SearchRepository :  ISearchRepository
{
    private readonly AppParameter _parameter;
    
    public SearchRepository(AppParameter parameter)
    {
        _parameter = parameter;
    }
    
    public async Task<Media> SearchById(int id)
    {
        try
        {
          
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"query MediaSearch($id: Int){

                    Media(id: $id){
                        id
                        title{
                            romaji,
                            english,
                            native
                        }
                        type,
                        format,
                        description,
                        season,
                        seasonYear,
                        episodes,
                        averageScore,
                        meanScore,
                        genres,
    	                studios {
                            nodes {
                                name,
                                isAnimationStudio
                            }
    	                }
                        startDate {
                            year,
                            month,
                            day
                        }
                        endDate {
                            year,
                            month,
                            day
                        }
                        status,
                        chapters,
                        volumes
                    }
                }",
                OperationName = "MediaSearch",
                Variables = new { id = id }
            };

            using GraphQLHttpClient client = new GraphQLHttpClient(_parameter.ApiEndpoint!, new NewtonsoftJsonSerializer());
            GraphQLResponse<MediaData> response = await client.SendQueryAsync<MediaData>(request);
            return response.Data.Media ?? throw new Exception("Page was null.");
        }
        catch (GraphQLHttpRequestException requestException)
        {
            Console.WriteLine("An error occurred while searching." +requestException.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while searching." +e);
            throw;
        }
    }

    public async Task<Page> SearchBySearchString(string searchQuery, int page, int perPage)
    {
        try
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"query PageSearch($page: Int $perPage: Int $search: String){
                        Page(page: $page perPage: $perPage){
                            pageInfo{
                                perPage,
                                currentPage,
                                lastPage,
                                hasNextPage,
                            }
                            media(search: $search){
                                id,
                                type,
                                title{
                                    romaji,
                                    english,
                                    native
                                }
                            }                        
                        }
                }",
                OperationName = "PageSearch",
                Variables = new { page = page, perPage = perPage, search = searchQuery }
            };
            
            using GraphQLHttpClient client = new GraphQLHttpClient(_parameter.ApiEndpoint!, new NewtonsoftJsonSerializer());
            GraphQLResponse<PageData> response = await client.SendQueryAsync<PageData>(request);
            return response.Data.Page ?? throw new Exception("Page was null.");
        }
        catch (GraphQLHttpRequestException requestException)
        {
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