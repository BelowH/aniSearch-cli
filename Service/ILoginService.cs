namespace aniList_cli.Service;

public interface ILoginService
{
    public string GetToken();

    public string GetUserId();
}