using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;

public class SearchMedia
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "title")] 
    public Title Title { get; set; } = new Title();

    [JsonProperty(PropertyName = "type")]
    public MediaType Type { get; set; }

    public string GetTitle()
    {
        string titleString = "";
        if (Title.English != null)
        {
            titleString = Title.English;
        }
        else if (Title.Romanji != null)
        {
            titleString = Title.Romanji;
        }else if (Title.Native != null)
        {
            titleString = Title.Native;
        }
        return titleString;
    }

    public override string ToString()
    {
        string type = Type switch
        {
            MediaType.ANIME => "(Anime)",
            MediaType.MANGA => "(Manga)",
            _ => ""
        };
        return type + GetTitle();
    }

    public bool TitleMatches(string title)
    {
        return ToString().Equals(title, StringComparison.InvariantCultureIgnoreCase);
    }
}