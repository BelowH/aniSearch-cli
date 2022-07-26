using Spectre.Console;

namespace aniList_cli.Gui.CustomList;

public class CustomList<T>
{
    private List<ListItem<T>> _items;

    private string _title;
    
    private string _controls;

    private int _pointer;

    private int _length;

    private int _size;

    private int _lastItemToDisplay;

    
    
    public CustomList(List<ListItem<T>> items, string markupTitle, string markupControls, int size = 0)
    {
        _controls = markupControls;
        _title = markupTitle;
        _pointer = 0;
        _length = items.Count;
        _items = items;
        _size = size;
        _lastItemToDisplay = _size > _length ? _length : _size;
    }

    public void Display()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Rule title = new Rule(_title);
        title.Alignment = Justify.Left;
        AnsiConsole.Write(title);

      
        
        for (int i = 0; i < _length; i++)
        {
            if (i == _pointer)
            {
                _items[i].Select();
            }
            else
            {
                _items[i].UnSelect(); 
            }
            _items[i].Display();
        }
        
        AnsiConsole.MarkupLine(_controls);
    }

    public void Up()
    {
        if (_pointer > 0)
        {
            _pointer--;
        }
        else
        {
            _pointer = _length -1;
        }
        Display();
    }

    public void Down()
    {
        if (_pointer < _length -1)
        {
            _pointer++;
        }
        else
        {
            _pointer = 0;
        }
        Display();
    }

    public T Select()
    {
        return _items[_pointer].GetValue();
    }
}