using Spectre.Console;

namespace aniList_cli.Gui.CustomList;

public class ListItem<T>
{
    private readonly T _value;

    public bool IsSelected;

    private string _color;

    public ListItem(T value, string color = "yellow")
    {
        _color = color;
        _value = value;
        IsSelected = false;
    }

    public T GetValue()
    {
        return _value;
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void UnSelect()
    {
        IsSelected = false;
    }
    
    public void Display()
    {
        if (IsSelected)
        {
            AnsiConsole.MarkupLine("["+_color+"](\u2192)\t[/][black on " + _color + "]" +Markup.Escape( _value?.ToString() ?? "" )+ "[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[" + _color + "]()\t" + Markup.Escape( _value?.ToString() ?? "" ) + "[/]");
        }
    }
    
    
    
}