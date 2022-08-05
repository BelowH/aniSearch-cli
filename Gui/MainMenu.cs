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
        _searchPage.OnBack += (_, _) => Display();
        _userPage = userPage;
        _mediaListPage = mediaListPage;
        _mediaListPage.OnBack += (_, _) => Display();
    }

    private const string SSearch = "Search";
    private const string SAnime = "Anime";
    private const string SManga = "Manga";
    private const string SProfile = "Profile";
    
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
                case ConsoleKey.E:
                    Exit();
                    return;
            }
        }
    }

    private void EnterSelection(string selection)
    {
        switch (selection)
        {
            case SSearch:
                _searchPage.Display();
                break;
            case SAnime:
                _mediaListPage.Display(MediaType.ANIME);
                break;
            case SManga:
                _mediaListPage.Display(MediaType.MANGA);
                break;
            case SProfile:
                _userPage.Display();
                break;
            default:
                //should not happen
                throw new ArgumentException("choice not found");
        }
        Display();
    }
    
    
    private void Exit()
    {
        Console.Clear();
        Environment.Exit(0);
    }
}