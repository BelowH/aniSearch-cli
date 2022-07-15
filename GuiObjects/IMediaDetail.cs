namespace aniList_cli.GuiObjects;

public interface IMediaDetail
{
    public void Display(int id);

    public event EventHandler OnBack;
}