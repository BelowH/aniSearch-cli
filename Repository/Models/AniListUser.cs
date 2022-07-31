using System.Text.Json.Serialization;

namespace aniList_cli.Repository.Models;

public class AniListUserData
{

    public AniListUser? User { get; set; }
    
}

public class AniListUser
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("about")]
    public string? AboutAsHtml { get; set; }

    [JsonPropertyName("siteUrl")]
    public string? SiteUrl { get; set; }

    [JsonPropertyName("updatedAt")]
    public int? UpdatedAt { get; set; }

    [JsonPropertyName("statistics")]
    public UserStatisticTypes? StatisticTypes { get; set; }
}

public class UserStatisticTypes
{
    [JsonPropertyName("anime")]
    public UserStatistics? AnimeStatistics { get; set; }

    [JsonPropertyName("manga")]
    public UserStatistics? MangaStatistics { get; set; }
}

public class UserStatistics
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("meanScore")]
    public float MeanScore { get; set; }

    [JsonPropertyName("minutesWatched")]
    public int MinutesWatched { get; set; }

    [JsonPropertyName("episodesWatched")]
    public int EpisodesWatched { get; set; }


    [JsonPropertyName("chaptersRead")]
    public int ChaptersRead { get; set; }
    
    [JsonPropertyName("volumesRead")]
    public int VolumesRead { get; set; }
    
    [JsonPropertyName("statuses")]
    public UserStatusStatistic[]? Statistic { get; set; }
}

public class UserStatusStatistic
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    
    [JsonPropertyName("meanScore")]
    public float MeanScore { get; set; }
    
    [JsonPropertyName("minutesWatched")]
    public int MinutesWatched { get; set; }
    
    [JsonPropertyName("chaptersRead")]
    public int ChaptersRead { get; set; }
    
    [JsonPropertyName("status")]
    public MediaListStatus MediaListStatus { get; set; }
    
    [JsonPropertyName("mediaIds")]
    public int[] MediaIds { get; set; } = Array.Empty<int>();
}
