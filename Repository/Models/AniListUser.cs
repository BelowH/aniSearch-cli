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

    [JsonProperty(PropertyName = "statistics")]
    public UserStatisticTypes? StatisticTypes { get; set; }
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
    [JsonProperty(PropertyName = "count")]
    public int Count { get; set; }

    [JsonProperty(PropertyName = "meanScore")]
    public float MeanScore { get; set; }

    [JsonProperty(PropertyName = "minutesWatched")]
    public int MinutesWatched { get; set; }

    [JsonProperty(PropertyName = "episodesWatched")]
    public int EpisodesWatched { get; set; }

    [JsonProperty(PropertyName = "chaptersRead")]
    public int ChaptersRead { get; set; }

    [JsonProperty(PropertyName = "volumesRead")]
    public int VolumesRead { get; set; }

    [JsonProperty(PropertyName = "statuses")]
    public UserStatusStatistic[]? Statistic { get; set; }
}

public class UserStatusStatistic
{

    [JsonProperty(PropertyName = "count")]
    public int Count { get; set; }

    [JsonProperty(PropertyName = "meanScore")]
    public float MeanScore { get; set; }

    [JsonProperty(PropertyName = "minutesWatched")]
    public int MinutesWatched { get; set; }
    
    [JsonProperty(PropertyName = "chaptersRead")]
    public int ChaptersRead { get; set; }

    [JsonProperty(PropertyName = "status")]
    public MediaListStatus MediaListStatus { get; set; }
    
}
