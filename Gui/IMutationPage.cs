using aniList_cli.Repository.Models;

namespace aniList_cli.Gui;

public interface IMutationPage
{

    public event EventHandler OnBack;
    
    public void MoveToList( int mediaId, MediaStatusInfo? info = null);
    
    public void AddProgress(Media media, MediaStatusInfo mediaStatusInfo, bool volume = false);

}