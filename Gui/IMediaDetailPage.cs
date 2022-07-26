namespace aniList_cli.Gui;

public interface IMediaDetailPage
{
    public void Display(int id, bool isInList = false);

    public void Back();

    public void AddToWatchlist();
    
    public event EventHandler OnBack;
}