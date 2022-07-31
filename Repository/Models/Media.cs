using System.Text.Json.Serialization;

namespace aniList_cli.Repository.Models;

public class MediaData
{

    [JsonPropertyName("Media")]
    public Media Media { get; set; } = new Media();
}


public class Media
{
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    
    [JsonPropertyName("title")]
    public Title Title { get; set; } = new Title();

 
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("startData")]
    public MediaDate? StartDate { get; set; }
    
    [JsonPropertyName("endDate")]
    public MediaDate? EndDate { get; set; }

    
    [JsonPropertyName("season")]
    public MediaSeason? Season { get; set; }

    [JsonPropertyName("seasonYear")]
    public int? SeasonYear { get; set; }

    [JsonPropertyName("type")]
    public MediaType Type { get; set; }

    [JsonPropertyName("format")]
    public MediaFormat? Format { get; set; }

    [JsonPropertyName("status")]
    public MediaStatus? Status { get; set; }

    [JsonPropertyName("studios")]
    public MediaStudio? Studio { get; set; }

    [JsonPropertyName("genres")]
    public string[]? Genres { get; set; }

    [JsonPropertyName("episodes")]
    public int? Episodes { get; set; }
    
    [JsonPropertyName("chapters")]
    public int? Chapters { get; set; }

    [JsonPropertyName("volumes")]
    public int? Volumes { get; set; }

    [JsonPropertyName("averageScore")]
    public int? AverageScore { get; set; }

    [JsonPropertyName("meanScore")]
    public int? MeanScore { get; set; }
    
}

public class MediaDate
{
    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("month")]
    public int? Month { get; set; }
    
    [JsonPropertyName("day")]
    public int?  Day { get; set; }
}

public class MediaStudio
{

    [JsonPropertyName("nodes")]
    public Node[]? Nodes { get; set; }
    
    public class Node
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isAnimationStudio")]
        public bool IsAnimationStudio { get; set; }
    }
    
}

