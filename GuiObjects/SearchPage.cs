using aniList_cli.Repository;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.GuiObjects;

public class SearchPage : IPage
{
    private SearchRepository _searchRepository;

    private const string BACK = "[red]Return to Menu[/]";
    private const string NEXT_PAGE = "[yellow]Next Page[/]";
    private const string NEW_SEARCH = "[green]New Search[/]";
    private const string LAST_PAGE = "[yellow]Last Page[/]";
    
    public SearchPage()
    {
        _searchRepository = new SearchRepository();
    }
    
    public void Display()
    {
        Console.Clear();
        
        Rule pageTitle = new Rule("[bold blue]Search![/]");
        pageTitle.Alignment = Justify.Left;
        AnsiConsole.Write(pageTitle);
        string searchPromt = AnsiConsole.Ask<string>("[bold]Search:[/]");
        const int pageSize = 5;
        int page = 1;

        QueryRepository(searchPromt, page, pageSize);

    }
    

    private void QueryRepository(string searchPromt, int page, int pageSize)
    {
        Page response = new Page();
        AnsiConsole.Status().Start(
            "Searching",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                response = _searchRepository.SearchBySearchString(searchPromt, page, pageSize).Result;
            }
        );

        if (response.Media.Length < 1)
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
        List<string> mediaStrings = response.Media.Select(medium => medium.ToString()).ToList();
        mediaStrings.Add(NEXT_PAGE);
        mediaStrings.Add(BACK);
        if (response.PageInfo.HasNextPage)
        {
            mediaStrings.Add(NEW_SEARCH);
        }

        if (page > 1)
        {
            mediaStrings.Add(LAST_PAGE);
        }

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Page: " + page+"[/]")
                .AddChoices(mediaStrings)
        );
        switch (choice)
        {
            case BACK:
                Back();
                break;
            case NEW_SEARCH:
                Display();
                break;
            case NEXT_PAGE:
                int pageInc = page + 1;
                QueryRepository(searchPromt,pageInc,pageSize);
                break;
            case LAST_PAGE:
                int pageDec = page - 1;
                if (pageDec < 1)
                {
                    pageDec = 1;
                }
                QueryRepository(searchPromt,pageDec,pageSize);
                break;
        }

        int id = response.Media.FirstOrDefault(x => x.TitleMatches(choice))!.Id;
        {
            MediaDetail detail = new MediaDetail(id);
            detail.Display();
        }
    }
    
    

    public void Back()
    {
        MainMenu menu = new MainMenu();
        menu.Display();
    }
}