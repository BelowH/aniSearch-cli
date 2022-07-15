using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;

public class PageInfo
{
    [JsonProperty(PropertyName = "perPage")]
    public int PerPage { get; set; } 

    [JsonProperty(PropertyName = "currentPage")]
    public int CurrentPage { get; set; }

    [JsonProperty(PropertyName = "lastPage")]
    public int LastPage { get; set; } = 1;

    [JsonProperty(PropertyName = "hasNextPage")]
    public bool HasNextPage { get; set; } = false;
}