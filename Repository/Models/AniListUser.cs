using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;

public class AniListUserData
{

    public AniListUser? User { get; set; }
    
}

public class AniListUser
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = null!;

    [JsonProperty(PropertyName = "about")]
    public string? AboutAsHtml { get; set; }

    [JsonProperty(PropertyName = "siteUrl")]
    public string? SiteUrl { get; set; }

    [JsonProperty(PropertyName = "updatedAt")]
    public int? UpdatedAt { get; set; }
}

public class UserStatisticTypes
{
    [JsonProperty(PropertyName = "anime")]
    public UserStatistics? AnimeStatistics { get; set; }

    [JsonProperty(PropertyName = "manga")]
    public UserStatistics? MangaStatistics { get; set; }
}

public class UserStatistics
{
    
}
