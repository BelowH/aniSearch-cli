using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using Spectre.Console;

namespace aniList_cli.Gui;

public class SearchPage : ISearchPage
{

    public event EventHandler? OnBackToMenu;
    
    private readonly ISearchRepository _searchRepository;

    private readonly IMediaDetailPage _mediaDetailPage;

    private Page? _currentPage;

    private int _currentPageNumber;
    
    private const int PageSize = 10;

    private string? _searchPrompt;
    
    private const string SBack = "[red]Return to Menu[/]";
    private const string SNextPage = "[yellow]Next Page[/]";
    private const string SNewSearch = "[green]New Search[/]";
    private const string SLastPage = "[yellow]Last Page[/]";
    
    public SearchPage(ISearchRepository searchRepository, IMediaDetailPage mediaDetailPage)
    {
        _currentPage = null;
        _searchRepository = searchRepository;
        _mediaDetailPage = mediaDetailPage;
        _mediaDetailPage.OnBack += (_, _) => DisplayResult();
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
                }
            }
            
            List<string> mediaStrings = _currentPage.Media.Select(medium => medium.ToString()).ToList();
            if (_currentPage.PageInfo.HasNextPage)
            {
                mediaStrings.Add(SNextPage);
            }
            if (_currentPageNumber > 1)
            {
                mediaStrings.Add(SLastPage);
            }
            mediaStrings.Add(SBack);
            if (_currentPage.PageInfo.HasNextPage)
            {
                mediaStrings.Add(SNewSearch);
            }

            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Page: " + _currentPageNumber + "[/]")
                    .AddChoices(mediaStrings)
            );
            switch (choice)
            {
                case SBack:
                    Back();
                    break;
                case SNewSearch:
                    Display();
                    break;
                case SNextPage:
                    Search(_searchPrompt, _currentPageNumber++);
                    DisplayResult();
                    break;
                case SLastPage:
                    _currentPageNumber--;
                    if (_currentPageNumber < 1)
                    {
                        _currentPageNumber = 1;
                    }
                    Search(_searchPrompt, _currentPageNumber);
                    DisplayResult();
                    break;
            }

            int id = _currentPage.Media.FirstOrDefault(x => x.TitleMatches(choice))!.Id;
            {
                _mediaDetailPage.Display(id);
            }
        }
        else
        {
            Display();
        }
        
    }
    
    private void Search(string searchPrompt, int page)
    {
        AnsiConsole.Status().Start(
            "Searching",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                _currentPage = _searchRepository.SearchBySearchString(searchPrompt, page, PageSize).Result;
            }
        );
    }


    private void Back()
    {
        EventHandler? handler = OnBackToMenu;
        handler?.Invoke(this, EventArgs.Empty);

    }
}