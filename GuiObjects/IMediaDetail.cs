namespace aniList_cli.GuiObjects;

public interface IMediaDetail
{
    public void Display(int id);

    public void Back();

    public void AddToWatchlist();
    
    public event EventHandler OnBack;
}