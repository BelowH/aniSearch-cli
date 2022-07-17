using System.Reflection;
using aniList_cli.Settings;
using Newtonsoft.Json;

namespace aniList_cli.Helper;

public static class FileHelper
{
    /// <summary>
    ///  read and parse settings.json
    /// </summary>
    /// <returns>Instance of AppParameter</returns>
    public static AppParameter ReadAppParameter()
    {
        try
        {

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "Config" +
                          Path.DirectorySeparatorChar + "settings.json";

            string fileContent = File.ReadAllText(path);
            AppParameter appParameter = JsonConvert.DeserializeObject<AppParameter>(fileContent);

            return appParameter;
        }
        catch (Exception)
        {
            //TODO this wierd ...
            Console.WriteLine("Error while reading settings file. :/ " +Environment.NewLine + " Using default settings. Some features might not work.");
            AppParameter parameter = new()
            {
                
                ApiClientId = "8875",
                ApiAuthEndpoint = "https://graphql.anilist.co/",
                ApiEndpoint = "",
                MainColorHex = null,
                HighlightColorHex = null,
                Button1ColorHex = null,
                Button2ColorHex = null,
                Button3ColorHex = null
            };
            return parameter;
        }
    }
}