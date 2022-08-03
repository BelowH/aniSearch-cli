using aniList_cli.Gui.CustomList;
using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.Models;

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
        OnBackToMedia();
    }

    public void AddProgress(MediaType type, int mediaId, int? amount)
    {
        throw new NotImplementedException();
    }

    private void OnBackToMedia()
    {
        EventHandler? handler = OnBack;
        handler?.Invoke(this,EventArgs.Empty);
    }
}