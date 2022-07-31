using System.Text.Json.Serialization;


namespace aniList_cli.Settings;

public class AppParameter
{
    
    [JsonPropertyName("apiClientID")]
    public string? ApiClientId { get; set; }
    
    [JsonPropertyName("apiEndpoint")]
    public string? ApiEndpoint { get; set; }

   
    [JsonPropertyName("apiAuthEndpoint")]
    public string? ApiAuthEndpoint { get; set; }

 
    [JsonPropertyName("mainColorHex")]
    public string? MainColorHex
    {
        init => MainColor = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.White);
    }


    [JsonPropertyName("highlightColorHex")]
    public string? HighlightColorHex
    {
        init =>   HighlightColor = !string.IsNullOrWhiteSpace(value) 
            ? new CustomConsoleColor(value) 
            : new CustomConsoleColor(ConsoleColor.Blue);
    }

    [JsonPropertyName("button1ColorHex")]
    public string? Button1ColorHex
    {
        init => Button1Color = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.Green);
    }
    
    [JsonPropertyName("button2ColorHex")]
    public string? Button2ColorHex
    {
        init => Button2Color = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.Red);
    }
    
    [JsonPropertyName("button3ColorHex")]
    public string? Button3ColorHex
    {
        init => Button3Color = !string.IsNullOrWhiteSpace(value)
            ? new CustomConsoleColor(value)
            : new CustomConsoleColor(ConsoleColor.Yellow);
    }
    
   
    [JsonPropertyName("saveAuthToken")]
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