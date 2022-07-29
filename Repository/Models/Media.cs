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
    public int Id { get; set; }

    [JsonProperty(PropertyName = "title")] 
    [JsonPropertyName("title")]
    public Title Title { get; set; } = new Title();

    [JsonProperty(PropertyName = "description")]
    public string? Description { get; set; }

    [JsonProperty(PropertyName = "startData")]
    public MediaDate? StartDate { get; set; }

    [JsonProperty(PropertyName = "endDate")]
    public MediaDate? EndDate { get; set; }

    [JsonProperty(PropertyName = "season")]
    public MediaSeason? Season { get; set; }

    [JsonProperty(PropertyName = "seasonYear")]
    public int? SeasonYear { get; set; }

    [JsonProperty(PropertyName = "type")]
    public MediaType Type { get; set; }

    [JsonProperty(PropertyName = "format")]
    public MediaFormat? Format { get; set; }

    [JsonProperty(PropertyName = "status")]
    public MediaStatus? Status { get; set; }

    [JsonProperty(PropertyName = "studios")]
    public MediaStudio? Studio { get; set; }

    [JsonProperty(PropertyName = "genres")]
    public string[]? Genres { get; set; }

    [JsonProperty(PropertyName = "episodes")]
    public int? Episodes { get; set; }
    
    [JsonProperty(PropertyName = "chapters")]
    public int? Chapters { get; set; }

    [JsonProperty(PropertyName = "volumes")]
    public int? Volumes { get; set; }

    [JsonProperty(PropertyName = "averageScore")]
    public int? AverageScore { get; set; }

    [JsonProperty(PropertyName = "meanScore")]
    public int? MeanScore { get; set; }
    
}

public class MediaDate
{
    [JsonProperty(PropertyName = "year")]
    public int? Year { get; set; }

    [JsonProperty(PropertyName = "month")]
    public int? Month { get; set; }
    
    [JsonProperty(PropertyName = "day")]
    public int?  Day { get; set; }
}

public class MediaStudio
{

    [JsonProperty(PropertyName = "nodes")]
    public Node[]? Nodes { get; set; }
    
    public class Node
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "isAnimationStudio")]
        public bool IsAnimationStudio { get; set; }
    }
    
}

