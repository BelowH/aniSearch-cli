using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MediaListPage : IMediaListPage
{

    private readonly IMediaListRepository _repository;

    public event EventHandler? OnBackToMenu;
    
    public MediaListPage(IMediaListRepository repository)
    {
        _repository = repository;
    }
    
    public void Display(MediaType type)
    {
        
        List<ListItem<string>> items = new List<ListItem<string>>() { new("test 1"),new("test 2"), new("test 3"), new("test 4") , new("test 5")};
        CustomList<string> list = new CustomList<string>(items,"Test","[red](R)eturn to Menu [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down  [/][green] (Enter) Select[/]" ,5);
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
            }
        }
        
    }

    private void OnBack()
    {
        EventHandler? handler = OnBackToMenu;
        handler?.Invoke(this, EventArgs.Empty);
    }

    
}