using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;

public class MediaData
{
    [JsonProperty(PropertyName = "Media")] 
    public Media Media { get; set; } = new Media();
}


public class Media
{
    [JsonProperty(PropertyName = "id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "title")] 
    [JsonPropertyName("title")]
    public Title Title { get; set; } = new Title();

    [JsonProperty(PropertyName = "description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonProperty(PropertyName = "startData")]
    [JsonPropertyName("startData")]
    public MediaDate? StartDate { get; set; }

    [JsonProperty(PropertyName = "endDate")]
    [JsonPropertyName("endDate")]
    public MediaDate? EndDate { get; set; }

    [JsonProperty(PropertyName = "season")]
    [JsonPropertyName("season")]
    public MediaSeason? Season { get; set; }

    [JsonProperty(PropertyName = "seasonYear")]
    [JsonPropertyName("seasonYear")]
    public int? SeasonYear { get; set; }

    [JsonProperty(PropertyName = "type")]
    [JsonPropertyName("type")]
    public MediaType Type { get; set; }

    [JsonProperty(PropertyName = "format")]
    [JsonPropertyName("format")]
    public MediaFormat? Format { get; set; }

    [JsonProperty(PropertyName = "status")]
    [JsonPropertyName("status")]
    public MediaStatus? Status { get; set; }

    [JsonProperty(PropertyName = "studios")]
    [JsonPropertyName("studios")]
    public MediaStudio? Studio { get; set; }

    [JsonProperty(PropertyName = "genres")]
    [JsonPropertyName("genres")]
    public string[]? Genres { get; set; }

    [JsonProperty(PropertyName = "episodes")]
    [JsonPropertyName("episodes")]
    public int? Episodes { get; set; }
    
    [JsonProperty(PropertyName = "chapters")]
    [JsonPropertyName("chapters")]
    public int? Chapters { get; set; }

    [JsonProperty(PropertyName = "volumes")]
    [JsonPropertyName("volumes")]
    public int? Volumes { get; set; }

    [JsonProperty(PropertyName = "averageScore")]
    [JsonPropertyName("averageScore")]
    public int? AverageScore { get; set; }

    [JsonProperty(PropertyName = "meanScore")]
    [JsonPropertyName("meanScore")]
    public int? MeanScore { get; set; }
    
}

public class MediaDate
{
    [JsonProperty(PropertyName = "year")]
    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonProperty(PropertyName = "month")]
    [JsonPropertyName("month")]
    public int? Month { get; set; }
    
    [JsonProperty(PropertyName = "day")]
    [JsonPropertyName("day")]
    public int?  Day { get; set; }
}

public class MediaStudio
{

    [JsonProperty(PropertyName = "nodes")]
    [JsonPropertyName("nodes")]
    public Node[]? Nodes { get; set; }
    
    public class Node
    {
        [JsonProperty(PropertyName = "name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "isAnimationStudio")]
        [JsonPropertyName("isAnimationStudio")]
        public bool IsAnimationStudio { get; set; }
    }
    
}

