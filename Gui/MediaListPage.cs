using System.Security.Authentication;
using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MediaListPage : IMediaListPage
{

    private readonly IAuthenticatedQueries _repository;

    private readonly IMediaDetailPage _mediaDetailPage;
    
    public event EventHandler? OnBackToMenu;

    private MediaListCollection? _mediaListCollection;

    private MediaType _type;

    private MediaList? _currentList;
    
    public MediaListPage(IAuthenticatedQueries repository, IMediaDetailPage mediaDetailPage)
    {
        _currentList = null;
        _mediaListCollection = null;
        _repository = repository;
        _mediaDetailPage = mediaDetailPage;
        _mediaDetailPage.OnBack += (_, _) => DisplayCurrentMediaList();
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
                        _mediaListCollection = _repository.GetMediaListByUserId(type)!;
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
                    _currentList = list.Select();
                    DisplayCurrentMediaList();
                    break;
            }
        }
    }

    private void DisplayCurrentMediaList()
    {
        if ( _currentList?.Entries == null || _currentList.Entries.Count == 0)
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

        string title = "[blue bold]" + _currentList + "[/]";
        
        List<ListItem<MediaListItem>> items = (from entry in _currentList.Entries where entry.Media != null select new ListItem<MediaListItem>(entry)).ToList();
        CustomList<MediaListItem> list = new CustomList<MediaListItem>(items, title, "[red](R)eturn to List [/][Yellow](\u2191)Up  [/][yellow](\u2193)Down  [/][yellow](\u2190)Previous Page[/] [yellow](\u2192)Next Page[/] [green](Enter) Select[/]", true);
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
                case ConsoleKey.LeftArrow:
                    list.PreviousPage();
                    break;
                case ConsoleKey.RightArrow:
                    list.NextPage();
                    break;
                case ConsoleKey.Enter:
                    MediaListItem listItem = list.Select();
                    _mediaDetailPage.Display(listItem.Media!.Id,true, _currentList.Status,listItem.Progress);
                    break;
            }
        }
    }
    
    private void OnBack()
    {
        _mediaListCollection = null;
        EventHandler? handler = OnBackToMenu;
        handler?.Invoke(this, EventArgs.Empty);
    }

    
}