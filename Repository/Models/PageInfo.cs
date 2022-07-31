

using System.Text.Json.Serialization;

namespace aniList_cli.Repository.Models;

public class PageInfo
{
    [JsonPropertyName("perPage")]
    public int PerPage { get; set; } 

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("lastPage")]
    public int LastPage { get; set; } = 1;

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; } = false;
}