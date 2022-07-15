using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace aniList_cli.Repository;

public class BaseRepository 
{
    private const string GraphQlUrl = "https://graphql.anilist.co/";

    protected readonly IGraphQLClient Client;

    protected BaseRepository()
    {
        Client = new GraphQLHttpClient(GraphQlUrl, new NewtonsoftJsonSerializer());
    }
    
}