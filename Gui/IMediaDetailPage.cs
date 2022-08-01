using aniList_cli.Repository.Models;

namespace aniList_cli.Gui;

public interface IMediaDetailPage
{
    public void Display(int id, bool isInList = false, MediaListStatus? userStatus = null, int progress = 0);

    public void Back();

    public void AddToWatchlist();
    
    public event EventHandler OnBack;
}