using aniList_cli.Repository;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.GuiObjects;

public class MainMenu : IPage
{

    private readonly SearchRepository _searchRepository;
    
    public MainMenu()
    {
        _searchRepository = new SearchRepository();
    }

    private const string SEARCH = "Search";
    private const string MY_LIST = "My List";
    private const string LOGIN = "Login";
    private const string PROFILE = "Profile";
    private const string SETTINGS = "Settings";
    private const string EXIT = "[red]Exit[/]";
    

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
                        SEARCH,
                        MY_LIST,
                        PROFILE,
                        LOGIN,
                        SETTINGS,
                        EXIT
                    })
            );
            switch (choice)
            {
                case SEARCH :
                    Search();
                    break;
                case MY_LIST:
                    My_List();
                    break;
                case PROFILE:
                    Profile();
                    break;
                case LOGIN:
                    Login();
                    break;
                case SETTINGS:
                    Settings();
                    break;
                case EXIT:
                    Environment.Exit(0);
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

    public void Back()
    {
        Display();
    }

    private void Search()
    {
        SearchPage searchPage = new SearchPage();
        searchPage.Display();
    }

    private void My_List()
    {
        Console.WriteLine("Not implemented yet :(");
        Display(); 
    }
    
    private void Login()
    {
        Console.WriteLine("Not implemented yet :(");
        Display();
    }

    private void Profile()
    {
        Console.WriteLine("Not implemented yet :(");
        Display();
    }

    private void Settings()
    {
        Console.WriteLine("Not implemented yet :(");
        Display();
    }
    
}