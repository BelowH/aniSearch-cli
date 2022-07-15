using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace aniList_cli.Repository;

public class BaseRepository
{
    protected const string graphQlUrl = "https://graphql.anilist.co/";

    protected IGraphQLClient _client;
    
    public BaseRepository()
    {
        _client = new GraphQLHttpClient(graphQlUrl, new NewtonsoftJsonSerializer());
    }
    
}