using Newtonsoft.Json;

namespace aniList_cli.Settings;

public class AppParameter
{
    
    [JsonProperty(PropertyName = "apiClientID")]
    public string? ApiClientId { get; set; }

    [JsonProperty(PropertyName = "apiEndpoint")]
    public string? ApiEndpoint { get; set; }

    [JsonProperty(PropertyName = "apiAuthEndpoint")]
    public string? ApiAuthEndpoint { get; set; }

    [JsonProperty(PropertyName = "mainColorHex")]
    public string? MainColorHex
    {
        init => MainColor = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.White);
    }

    [JsonProperty(PropertyName = "highlightColorHex")]
    public string? HighlightColorHex
    {
        init =>   HighlightColor = !string.IsNullOrWhiteSpace(value) 
            ? new CustomConsoleColor(value) 
            : new CustomConsoleColor(ConsoleColor.Blue);
    }

    [JsonProperty(PropertyName = "button1ColorHex")]
    public string? Button1ColorHex
    {
        init => Button1Color = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.Green);
    }
    
    [JsonProperty(PropertyName = "button2ColorHex")]
    public string? Button2ColorHex
    {
        init => Button2Color = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.Red);
    }
    
    [JsonProperty(PropertyName = "button3ColorHex")]
    public string? Button3ColorHex
    {
        init => Button3Color = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.Yellow);
    }
    
    [JsonProperty(PropertyName = "saveAuthToken")]
    public bool SaveAuthToken { get; set; }
    
    [JsonIgnore]
    public CustomConsoleColor? MainColor { get; private init; }

    [JsonIgnore]
    public CustomConsoleColor? HighlightColor { get; private init; }

    [JsonIgnore]
    public CustomConsoleColor? Button1Color { get; private init; } 

    [JsonIgnore]
    public CustomConsoleColor? Button2Color { get; private init; } 
    
    [JsonIgnore]
    public CustomConsoleColor? Button3Color { get; private init; }

    

}