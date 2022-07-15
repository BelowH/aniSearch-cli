using Newtonsoft.Json;

namespace aniList_cli.Repository.Models;

public class SearchMedia
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "title")] 
    public Title Title { get; set; } = new Title();

    public override string ToString()
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

    public bool TitleMatches(string title)
    {
        if (Title.English != null && Title.English.Equals(title,StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }
        else if (Title.Romanji != null && Title.Romanji.Equals(title,StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }
        else if (Title.Native != null && Title.Native.Equals(title,StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return false;
    }
}