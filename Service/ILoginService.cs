namespace aniList_cli.Service;

public interface ILoginService
{
    public string GetToken();

    public bool IsLoggedIn();

    public string NewToken();
}