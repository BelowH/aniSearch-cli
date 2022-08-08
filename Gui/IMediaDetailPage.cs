using aniList_cli.Repository.Models;

namespace aniList_cli.Gui;

public interface IMediaDetailPage
{
    public void DisplayMedia(int id, IMainMenu.Callback caller);
    
}