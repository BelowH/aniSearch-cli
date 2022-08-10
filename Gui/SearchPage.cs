using System.Diagnostics.CodeAnalysis;
using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using Spectre.Console;

namespace aniList_cli.Gui;

public class SearchPage : ISearchPage
{
    private readonly IUnAuthenticatedQueries _unAuthenticatedQueries;
    private readonly IMediaDetailPage _mediaDetailPage;

    private Page? _currentPage;
    private int _currentPageNumber;
    private const int PageSize = 10;
    private string? _searchPrompt;
    
    public event EventHandler? OnBack;
    
    public SearchPage(IUnAuthenticatedQueries unAuthenticatedQueries, IMediaDetailPage mediaDetailPage)
    {
        _currentPage = null;
        _unAuthenticatedQueries = unAuthenticatedQueries;
        _mediaDetailPage = mediaDetailPage;
    }
    
    public void Display()
    {
        Console.Clear();
        _currentPageNumber = 1;   
        Rule pageTitle = new Rule("[bold blue]Search![/]");
        pageTitle.Alignment = Justify.Left;
        AnsiConsole.Write(pageTitle);
        _searchPrompt = AnsiConsole.Ask<string>("[bold]Search:[/]");

        SearchAndDisplayPage();
    }

    

    private void SearchAndDisplayPage()
    {
        if (_searchPrompt == null)
        {
            Display();
            return;
        }
        Search(_searchPrompt, _currentPageNumber);
        DisplayResult();
    }

    private void DisplayResult()
    {
        Console.Clear();
        Rule pageTitle = new Rule("[bold blue]Search Result:[/]");
        pageTitle.Alignment = Justify.Left;
        AnsiConsole.Write(pageTitle);
        if (_searchPrompt == null)
        {
            Display();
            return;
        }
        if (_currentPage != null)
        {
            if (_currentPage.Media.Length < 1)
            {
                AnsiConsole.MarkupLine("[bold red]No[/] results found");
                if (AnsiConsole.Confirm("try again?"))
                {
                    Display();
                }
                else
                {
                    Back();
                    return;
                }
            }
            
            List<ListItem<SearchMedia>> items = new();
            foreach (SearchMedia media in _currentPage.Media)
            {
                string color = media.Type == MediaType.ANIME ? "green" : "blue";
                items.Add(new ListItem<SearchMedia>(media,color));
            }

            string listControls = "[red](R)eturn to menu [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down [/][green](N)ew Search[/]" + Environment.NewLine;
            if (_currentPageNumber != 1)
            {
                listControls += "[yellow](\u2190) previous Page[/]";
            }
            if (_currentPage.PageInfo.HasNextPage)
            {
                listControls += "[yellow](\u2192) next Page[/]";
            }
            listControls += Environment.NewLine + "[yellow]Page: " + _currentPageNumber + "[/]";
            CustomList<SearchMedia> searchList = new CustomList<SearchMedia>(items,"[bold blue]Search Result:[/]",listControls);
            searchList.Display();
            Console.CursorVisible = false;
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.R:
                        Back();
                        return;
                    case ConsoleKey.N:
                        Display();
                        break;
                    case ConsoleKey.RightArrow:
                        if (_currentPage.PageInfo.HasNextPage)
                        {
                            Search(_searchPrompt, ++_currentPageNumber);
                            DisplayResult();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        _currentPageNumber--;
                        if (_currentPageNumber < 1)
                        {
                            _currentPageNumber = 1;
                        }
                        Search(_searchPrompt, _currentPageNumber);
                        DisplayResult();
                        break;
                    case ConsoleKey.Enter:
                        IMainMenu.Callback callback = DisplayResult;
                         _mediaDetailPage.DisplayMedia(searchList.Select().Id,callback);
                         DisplayResult();
                        break;
                    case ConsoleKey.DownArrow:
                        searchList.Down();
                        break;
                    case ConsoleKey.UpArrow:
                        searchList.Up();
                        break;
                }
            }
        }
        Display();
    }
    
    private void Search(string searchPrompt, int page)
    {
        Console.CursorVisible = true;
        AnsiConsole.Status().Start(
            "Searching",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                _currentPage = _unAuthenticatedQueries.SearchBySearchString(searchPrompt, page, PageSize);
            }
        );
    }

    private void Back()
    {
        EventHandler? handler = OnBack;
        handler?.Invoke(this,EventArgs.Empty);
    }
}