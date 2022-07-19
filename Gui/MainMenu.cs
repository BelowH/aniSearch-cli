using Spectre.Console;

namespace aniList_cli.Gui;

public class MainMenu : IMainMenu
{
    
    private readonly ISearchPage _searchPage;

    private readonly IUserPage _userPage;
    
    public MainMenu( ISearchPage searchPage, IUserPage userPage)
    {
        _searchPage = searchPage;
        _userPage = userPage;
        _userPage.OnBackToMenu += (_, _) => Display();
        _searchPage.OnBackToMenu += (_,_) => Display();
    }

    private const string SSearch = "Search";
    private const string SMyList = "My List";
    private const string SProfile = "Profile";
    private const string SSettings = "Settings";
    private const string SExit = "[red]Exit[/]";
    

    public void Display()
    {
        try
        {
            Console.Clear();
            Rule title = new Rule("[bold blue]Welcome to aniList-cli![/]");
            title.Alignment = Justify.Left;
            AnsiConsole.Write(title);
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new []
                    {
                        SSearch,
                        SMyList,
                        SProfile,
                        SSettings,
                        SExit
                    })
            );
            switch (choice)
            {
                case SSearch :
                    Search();
                    break;
                case SMyList:
                    My_List();
                    break;
                case SProfile:
                    Profile();
                    break;
                case SSettings:
                    Settings();
                    break;
                case SExit:
                    Exit();
                    break;
                default:
                    //should not happen
                    throw new ArgumentException("choice not found");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private void Search()
    {
        _searchPage.Display();
    }

    private void My_List()
    {
        Console.WriteLine("Not implemented yet :(");
        Display(); 
    }
    

    private void Profile()
    {
        _userPage.Display();
    }

    private void Settings()
    {
        Console.WriteLine("Not implemented yet :(");
        Display();
    }

    private void Exit()
    {
        Console.Clear();
        Environment.Exit(0);
    }
}