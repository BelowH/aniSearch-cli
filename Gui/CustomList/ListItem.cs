using Spectre.Console;

namespace aniList_cli.Gui.CustomList;

public class ListItem<T>
{
    private readonly T _value;

    private bool _isSelceted;

    private string _color;

    public ListItem(T value, string color = "yellow")
    {
        _color = color;
        _value = value;
        _isSelceted = false;
    }

    public T GetValue()
    {
        return _value;
    }

    public void Select()
    {
        _isSelceted = true;
    }

    public void UnSelect()
    {
        _isSelceted = false;
    }
    
    public void Display()
    {
        if (_isSelceted)
        {
            AnsiConsole.MarkupLine("[black on " + _color + "](\u2192)\t" + _value + "[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[" + _color + "]()\t" + _value + "[/]");
        }
    }
    
    
    
}