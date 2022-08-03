using aniList_cli.Repository.Models;

namespace aniList_cli.Gui;

public interface IMediaDetailPage
{
    public void Display(int id);

    public void Back();
    
    public event EventHandler OnBack;
}