namespace aniList_cli.GuiObjects;

public interface ISearchPage
{
    public void Display();

    public event EventHandler OnBackToMenu;
}