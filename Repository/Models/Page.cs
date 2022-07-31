using System.Text.Json.Serialization;

namespace aniList_cli.Repository.Models;


public class PageData
{
    [JsonPropertyName("Page")]
    public Page? Page { get; set; }
}

public class Page
{

    [JsonPropertyName("media")]
    public SearchMedia[] Media { get; set; } = Array.Empty<SearchMedia>();


    [JsonPropertyName("pageInfo")]
    public PageInfo PageInfo { get; set; } = new PageInfo();
}