using System.Security.Authentication;
using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MediaListPage : IMediaListPage
{

    private readonly IMediaListRepository _repository;

    public event EventHandler? OnBackToMenu;

    private MediaListCollection? _mediaListCollection;

    private MediaType _type;
    
    public MediaListPage(IMediaListRepository repository)
    {
        _mediaListCollection = null;
        _repository = repository;
    }
    
    public void Display(MediaType type)
    {
        _type = type;
        string title = "[bold blue]" + type + " List[/]";


        if (_mediaListCollection == null)
        {
            Console.Clear();
            try
            {
                AnsiConsole.Status().Start(
                    "Loading List",
                    ctx =>
                    {
                        ctx.SpinnerStyle = new Style(Color.Blue);
                        _mediaListCollection = _repository.GetMediaListByUserId(type)!.Result;
                    }
                );
            }
            catch (AuthenticationException)
            {
                OnBack();
            }
        }
        if (_mediaListCollection?.Lists == null || _mediaListCollection.Lists.Count == 0)
        {
            AnsiConsole.MarkupLine("[red bold]No " + type +" List found.[/]");
            AnsiConsole.MarkupLine("[red](R)eturn to Menu [/]");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.R) continue;
                OnBack();
                return;
            }
            
        }
        List<ListItem<MediaList>> items = _mediaListCollection.Lists.Select(mediaList => new ListItem<MediaList>(mediaList)).ToList();
        CustomList<MediaList> list = new CustomList<MediaList>(items,title,"[red](R)eturn to Menu [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down  [/][green] (Enter) Select[/]" );
        list.Display();

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    list.Down();
                    break;
                case ConsoleKey.UpArrow:
                    list.Up();
                    break;
                case ConsoleKey.R:
                    OnBack();
                    break;
                case ConsoleKey.Enter:
                    DisplayMediaList(list.Select());
                    break;
            }
        }
        
    }

    private void DisplayMediaList(MediaList mediaList)
    {
        if (mediaList.Entries == null || mediaList.Entries.Count == 0)
        {
            AnsiConsole.MarkupLine("[red bold]Empty list.[/]");
            AnsiConsole.MarkupLine("[red](R)eturn[/]");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.R) continue;
                Display(_type);
                return;
            }
        }

        string title = "[blue bold]" + mediaList + "[/]";
        
        List<ListItem<string>> items = (from entry in mediaList.Entries where entry.Media != null select new ListItem<string>(entry.ToString())).ToList();
        CustomList<string> list = new CustomList<string>(items, title, "[red](R)eturn to Menu [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down  [/][green] (Enter) Select[/]");
        list.Display();
        
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    list.Down();
                    break;
                case ConsoleKey.UpArrow:
                    list.Up();
                    break;
                case ConsoleKey.R:
                    Display(_type);
                    break;
            }
        }
    }
    
    private void OnBack()
    {
        EventHandler? handler = OnBackToMenu;
        handler?.Invoke(this, EventArgs.Empty);
    }

    
}