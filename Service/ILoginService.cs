using System.IdentityModel.Tokens.Jwt;

namespace aniList_cli.Service;

public interface ILoginService
{
    public string GetToken();

    public string GetUserId();

    public bool IsUserLoggedIn();

    public JwtSecurityToken NewToken();
}