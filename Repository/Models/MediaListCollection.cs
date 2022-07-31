using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;


// ReSharper disable All

namespace aniList_cli.Repository.Models;


public class MediaListCollectionResponse
{

    [JsonPropertyName("MediaListCollection")]
    public MediaListCollection? MediaListCollection { get; set; }
}


public class MediaListCollection
{
    [JsonPropertyName("lists")]
    public List<MediaList>? Lists { get; set; }
}

public class MediaList
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MediaListStatus Status { get; set; }

    [JsonPropertyName("progress")]
    public int Progress { get; set; }
    
    [JsonPropertyName("entries")]
    public List<MediaListItem>? Entries { get; set; }

    public override string ToString()
    {
        string val = "";
        if (!string.IsNullOrWhiteSpace(Name))
        {

            val += Name;
        }
        else
        {
            val += Status.ToString();
        }
        if (Entries != null)
        {
            val += "(" + Entries.Count + ")";
        }
        return val;
    }
}

public class MediaListItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MediaListStatus Status { get; set; }
    
    [JsonPropertyName("progress")]
    public int Progress { get; set; }
    
    [JsonPropertyName("media")]
    public Media? Media { get; set; }


    public override string ToString()
    {
        string val = "No Media found.";
        
        if (Media != null)
        {
            int len = 0;
            if (Media.Type == MediaType.ANIME)
            {
                len = Media.Episodes ?? 0;
            }
            else
            {
                len = Media.Chapters ?? 0;
            }
            val = Media.Title.ToString() + " (" + Progress + "/" + len + ")";
        }

        return val;
    }
}