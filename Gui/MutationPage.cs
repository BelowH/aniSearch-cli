using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MutationPage : IMutationPage
{
    public event EventHandler? OnBack;

    private readonly IAuthenticatedQueries _authenticated;
    
    public MutationPage(IAuthenticatedQueries authenticated)
    {
        _authenticated = authenticated;
    }
    
    public void MoveToList(  int mediaId,MediaStatusInfo? info = null)
    {
        Console.Clear();
        List<ListItem<MediaListStatus>> listItems = new List<ListItem<MediaListStatus>>()
        {
            new(MediaListStatus.PAUSED),
            new(MediaListStatus.CURRENT),
            new(MediaListStatus.DROPPED),
            new(MediaListStatus.PLANNING),
            new(MediaListStatus.COMPLETED),
            new(MediaListStatus.REPEATING)
        };
        string controls = "[red](R)eturn [/][Yellow](\u2191) Up  [/][yellow](\u2193) Down  [/][green] (Enter) Select[/]";
        CustomList<MediaListStatus> list = new CustomList<MediaListStatus>(listItems, "[blue]Select a List[/]",controls);
        list.Display();
        MediaListStatus? status = null;
        while (status == null)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.R:
                    OnBackToMedia();
                    break;
                case ConsoleKey.UpArrow:
                    list.Up();
                    break;
                case ConsoleKey.DownArrow:
                    list.Down();
                    break;
                case ConsoleKey.Enter:
                    status = list.Select();
                    break;
            }
        }
        if (info == null)
        {
            info = new MediaStatusInfo();
            info.Progress = 0;
            info.Status = (MediaListStatus)status;
            _authenticated.AddMediaToList(info.Status,mediaId);
        }
        else
        {
            _authenticated.AddMediaToList((MediaListStatus)status,mediaId,info.Id);
        }
    }

    public void AddProgress(Media media, MediaStatusInfo mediaStatusInfo, bool volume = false)
    {
        int progress = 1;
        int maxProgress = int.MaxValue;
        if (volume)
        {
            if (media.Volumes is > 0)
            {
                maxProgress = media.Volumes ?? int.MaxValue;
            }
        }
        else
        {
            if (media.Episodes is > 0)
            {
                maxProgress = media.Episodes ?? int.MaxValue;
            }
        }

        Console.Clear();
        while (true)
        {
            
            string input = AnsiConsole.Ask<string>("How much do you want to add?","1");
            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }
            if (int.TryParse(input, out progress) )
            {
                if (progress <= maxProgress)
                {
                    break;
                }
                AnsiConsole.Markup("[red]Your input was to big.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Your input was not a number.[/]");
            }
            AnsiConsole.MarkupLine("[red]Press any Key to try again or (R) to go Back[/]");
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.R)
            {
                OnBackToMedia();
            }
        }
        if (volume)
        {
            
            _authenticated.SetVolumeProgress(media.Id,mediaStatusInfo.Id,mediaStatusInfo.ProgressVolumes ?? 0 + progress);
        }
        else
        {
            _authenticated.SetProgress(media.Id,mediaStatusInfo.Id,mediaStatusInfo.Progress ?? 0 + progress); 
        }
    }

    private void OnBackToMedia()
    {
        EventHandler? handler = OnBack;
        handler?.Invoke(this,EventArgs.Empty);
    }
}