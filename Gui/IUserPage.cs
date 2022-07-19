namespace aniList_cli.Gui;

public interface IUserPage
{

    public event EventHandler OnBackToMenu;
    
    public void Display();
}