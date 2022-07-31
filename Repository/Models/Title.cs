using System.Text.Json.Serialization;

namespace aniList_cli.Repository.Models;

public class Title
{
    [JsonPropertyName("romanji")]
    public string? Romanji { get; set; }

    [JsonPropertyName("english")]
    public string? English { get; set; }
    
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