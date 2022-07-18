namespace aniList_cli.Gui;

public interface IListPage
{

    public event EventHandler OnBackButtonPress;
    
    public void Display();
}