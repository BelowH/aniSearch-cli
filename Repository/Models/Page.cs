using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;


public class PageData
{
    [JsonProperty(PropertyName = "Page")]
    public Page? Page { get; set; }
}

public class Page
{

    [JsonProperty(PropertyName = "media")] 
    public SearchMedia[] Media { get; set; } = Array.Empty<SearchMedia>();


    [JsonProperty(PropertyName = "pageInfo")]
    public PageInfo PageInfo { get; set; } = new PageInfo();
}