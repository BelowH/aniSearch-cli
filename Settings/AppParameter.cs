using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;


namespace aniList_cli.Settings;

public class AppParameter
{

    public AppParameter(IConfiguration configuration)
    {
        ApiEndpoint = configuration["apiEndpoint"];
        ApiClientId = configuration["apiClientId"];
        ApiAuthEndpoint = configuration["apiAuthEndpoint"];
        SaveAuthToken = bool.Parse(configuration["saveAuthToken"]);
    }
    
    
    [JsonPropertyName("apiClientID")]
    public string? ApiClientId { get; set; }
    
    [JsonPropertyName("apiEndpoint")]
    public string? ApiEndpoint { get; set; }

   
    [JsonPropertyName("apiAuthEndpoint")]
    public string? ApiAuthEndpoint { get; set; }

    
    [JsonPropertyName("saveAuthToken")]
    public bool SaveAuthToken { get; set; }
    
    
}