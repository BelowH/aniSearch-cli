using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MainMenu : IMainMenu
{
    
    private readonly ISearchPage _searchPage;

    private readonly IUserPage _userPage;

    private readonly IMediaListPage _mediaListPage;
    
    public MainMenu( ISearchPage searchPage, IUserPage userPage, IMediaListPage mediaListPage)
    {
        _searchPage = searchPage;
        _userPage = userPage;
        _mediaListPage = mediaListPage;
        _userPage.OnBackToMenu += (_, _) => Display();
        _searchPage.OnBackToMenu += (_, _) => Display();
        _mediaListPage.OnBackToMenu += (_, _) => Display();
    }

    private const string SSearch = "Search";
    private const string SAnime = "Anime";
    private const string SManga = "Manga";
    private const string SProfile = "Profile";
    private const string SSettings = "Settings";
    private const string SExit = "[red]Exit[/]";
    

    public void Display()
    {
        Console.Clear();
        Rule title = new Rule("[bold blue]Welcome to aniList-cli![/]");
        List<ListItem<string>> mainMenuItems = new List<ListItem<string>>()
        {
            new(SSearch,"green"),
            new(SAnime,"blue"),
            new(SManga,"blue"),
            new(SProfile,"blue")
        };
        CustomList<string> mainList = new CustomList<string>(mainMenuItems,"[bold blue]Welcome to aniList-cli![/]", "[red](E)xit [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down  [/][green] (Enter) Select[/]");
        mainList.Display();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    mainList.Down();
                    break;
                case ConsoleKey.UpArrow:
                    mainList.Up();
                    break;
                case ConsoleKey.Enter:
                    EnterSelection(mainList.Select());
                    break;
                case ConsoleKey.R:
                    Exit();
                    break;
                
            }
        }
    }

    private void EnterSelection(string selection)
    {
        switch (selection)
        {
            case SSearch:
                Search();
                break;
            case SAnime:
                Anime();
                break;
            case SManga:
                Manga();
                break;
            case SProfile:
                Profile();
                break;
            default:
                //should not happen
                throw new ArgumentException("choice not found");
        }
        Display();
    }
    
    private void Search()
    {
        _searchPage.Display();
    }

    private void Anime()
    {
        _mediaListPage.Display(MediaType.ANIME);
    }

    private void Manga()
    {
        _mediaListPage.Display(MediaType.MANGA);
    }

    private void Profile()
    {
        _userPage.Display();
    }
    
    private void Exit()
    {
        Console.Clear();
        Environment.Exit(0);
    }
}