using aniList_cli.Repository.Models;

namespace aniList_cli.Gui;

public interface IMediaListPage
{
    public void Display(MediaType type);

    public event EventHandler OnBackToMenu;
}