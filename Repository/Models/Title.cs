using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;

public class Title
{
    [JsonProperty(PropertyName = "romanji")]
    [JsonPropertyName("romanji")]
    public string? Romanji { get; set; }

    [JsonProperty(PropertyName = "english")]
    [JsonPropertyName("english")]
    public string? English { get; set; }

    [JsonProperty(PropertyName = "native")]
    [JsonPropertyName("native")]
    public string? Native { get; set; }

    public override string ToString()
    {
        string titleString = "";
        if (English != null)
        {
            titleString = English;
        }
        else if (Romanji != null)
        {
            titleString = Romanji;
        }else if (Native != null)
        {
            titleString = Native;
        }
        return titleString;
    }
}