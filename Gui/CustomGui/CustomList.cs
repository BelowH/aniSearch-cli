using Spectre.Console;

namespace aniList_cli.Gui.CustomList;

public class CustomList<T>
{
    private readonly List<List<ListItem<T>>> _pages;

    private readonly string _title;
    
    private readonly string _controls;
    
    private readonly bool _pagination;
    
    private int _pointer;
    
    private int _page;
    
    public CustomList(List<ListItem<T>> items, string markupTitle, string markupControls, bool pagination = false, int pageSize = 15)
    {
        _controls = markupControls;
        _title = markupTitle;
        _pointer = 0;
        _page = 0;
        _pagination = pagination;
        if (!_pagination)
        {
            _pages = new List<List<ListItem<T>>> { items };
        }
        else
        {
            _pages = items.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / pageSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

    }

    public void Display()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Rule title = new Rule(_title)
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(title);

        for (int i = 0; i < _pages[_page].Count; i++)
        {
            if (i == _pointer)
            {
                _pages[_page][i].Select();
            }
            else
            {
                _pages[_page][i].UnSelect(); 
            }
            _pages[_page][i].Display();
        }

        string lowerRule = _controls;
        if (_pagination)
        {
            lowerRule += " Page: (" + (_page + 1) + " of " + (_pages.Count) + ")";
        }
        AnsiConsole.MarkupLine(lowerRule);
    }

    public void Up()
    {
        if (_pointer > 0)
        {
            _pointer--;
        }
        else
        {
            _pointer = _pages[_page].Count -1;
        }
        Console.WriteLine(_pointer);
        Display();
    }

    public void Down()
    {
        if (_pointer < _pages[_page].Count -1)
        {
            _pointer++;
        }
        else
        {
            _pointer = 0;
        }
        Display();
    }

    public void NextPage()
    {
        if (!_pagination) return;
        if (_page < _pages.Count -1)
        {
            _pointer = 0;
            _page++;
        }
        Display();
    }

    public void PreviousPage()
    {
        if(!_pagination) return;
        if (_page > 0)
        {
            _pointer = 0;
            _page--;
        }
        Display();
    }
    
    
    public T Select()
    {
        return _pages[_page][_pointer].GetValue();
    }
}