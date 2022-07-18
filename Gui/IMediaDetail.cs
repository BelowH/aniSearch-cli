namespace aniList_cli.Gui;

public interface IMediaDetail
{
    public void Display(int id);

    public void Back();

    public void AddToWatchlist();
    
    public event EventHandler OnBack;
}