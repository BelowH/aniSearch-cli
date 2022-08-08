using System.Text.Json.Serialization;

namespace aniList_cli.Repository.Models;

public class MediaStatusResponse
{
    [JsonPropertyName("MediaList")]
    public MediaStatusInfo? MediaStatusInfo { get; set; }    
}

public class MediaStatusInfo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("mediaId")]
    public int MediaId { get; set; }

    [JsonPropertyName("status")]
    public MediaListStatus Status { get; set; }

    [JsonPropertyName("progress")]
    public int? Progress { get; set; }
    
    [JsonPropertyName("progressVolumes")]
    public int? ProgressVolumes { get; set; }
}
