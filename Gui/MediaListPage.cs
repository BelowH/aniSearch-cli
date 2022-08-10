using System.Security.Authentication;
using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.Models;
using aniList_cli.Service;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MediaListPage : IMediaListPage
{

    private readonly IAuthenticatedQueries _repository;
    private readonly IMediaDetailPage _mediaDetailPage;
    private readonly ILoginService _login;
    
    private MediaListCollection? _mediaListCollection;
    private MediaType _type;
    private MediaListStatus? _currentListStatus;
    public event EventHandler? OnBack;
    
    public MediaListPage(IAuthenticatedQueries repository, IMediaDetailPage mediaDetailPage, ILoginService login)
    {
        _currentListStatus = null;
        _mediaListCollection = null;
        _repository = repository;
        _mediaDetailPage = mediaDetailPage;
        _login = login;
    }
    
    public void Display(MediaType type)
    {
        _type = type;
        string title = "[bold blue]" + type + " List[/]";


        if (_mediaListCollection == null)
        {
            LoadLists();
        }
        if (_mediaListCollection?.Lists == null || _mediaListCollection.Lists.Count == 0)
        {
            AnsiConsole.MarkupLine("[red bold]No " + type +" List found.[/]");
            AnsiConsole.MarkupLine("[red](R)eturn to Menu [/]");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.R) continue;
                return;
            }
            
        }
        List<ListItem<MediaListStatus>> items = _mediaListCollection.Lists.Select(mediaList => new ListItem<MediaListStatus>(mediaList.Status)).ToList();
        CustomList<MediaListStatus> list = new CustomList<MediaListStatus>(items,title,"[red](R)eturn to Menu [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down  [/][green] (Enter) Select[/]" );
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
                    Back();
                    return;
                case ConsoleKey.Enter:
                    _currentListStatus = list.Select();
                    DisplayCurrentMediaList();
                    Display(type);
                    return;
            }
        }
    }
    
    private void DisplayCurrentMediaList()
    {
        MediaList? selectedList  = _mediaListCollection?.Lists?.FirstOrDefault(list => list.Status == _currentListStatus);
        if (selectedList == null  || selectedList.Entries is { Count: 0 })
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

        string title = "[blue bold]" + _currentListStatus + "[/]";
        
        List<ListItem<MediaListItem>> items = (from entry in selectedList.Entries where entry.Media != null select new ListItem<MediaListItem>(entry)).ToList();
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
                    return;
                case ConsoleKey.LeftArrow:
                    list.PreviousPage();
                    break;
                case ConsoleKey.RightArrow:
                    list.NextPage();
                    break;
                case ConsoleKey.Enter:
                    MediaListItem listItem = list.Select();
                    void Callback()
                    {
                        LoadLists();
                        DisplayCurrentMediaList();
                    }
                    _mediaDetailPage.DisplayMedia(listItem.Media!.Id,Callback);
                    break;
            }
        }
    }
    
    private void LoadLists()
    {
        Console.Clear();
        try
        {
            string userId = _login.GetUserId();
                
            AnsiConsole.Status().Start(
                "Loading List",
                ctx =>
                {
                    ctx.SpinnerStyle = new Style(Color.Blue);
                    _mediaListCollection = _repository.GetMediaListByUserId(_type,userId)!;
                }
            );
        }
        catch (AuthenticationException)
        {
            Back();
        }   
    }

    private void Back()
    {
        _mediaListCollection = null;
        EventHandler? handler = OnBack;
        handler?.Invoke(this,EventArgs.Empty);
    }
    
}