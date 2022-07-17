using System.Drawing;
using System.Globalization;

namespace aniList_cli.Settings;

public class CustomConsoleColor
{

    public CustomConsoleColor(string hex)
    {
        Color = FromHex(hex);
    }

    public CustomConsoleColor(ConsoleColor color)
    {
        Color = color;
    }
    
    public ConsoleColor Color {get;}


    private static ConsoleColor FromHex(string hex)
    {
        
        ColorConverter converter = new ColorConverter();
        Color c = (Color)(converter.ConvertFromString(hex) ?? throw new InvalidOperationException());
        int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
        index |= (c.R > 64) ? 4 : 0; // Red bit
        index |= (c.G > 64) ? 2 : 0; // Green bit
        index |= (c.B > 64) ? 1 : 0; // Blue bit
        
        return (ConsoleColor)index;
    }
}