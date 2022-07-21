using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using aniList_cli.Settings;
using Spectre.Console;

namespace aniList_cli.Service;

public class LoginService : ILoginService
{

    private readonly AppParameter _parameter;

    private JwtSecurityToken? _token;

    private string? _userId;
    
    public LoginService(AppParameter parameter)
    {

        try
        {
            AuthToken token = AuthToken.Load();
            if (token.ExpireDate > DateTime.UtcNow)
            {
                _token = new JwtSecurityToken(token.Token);
                _userId = _token.Subject;
            }
        }
        catch (Exception)
        {
            _token = null;
            _userId = null;
        }
        _parameter = parameter;
        
    }
    
    /// <summary>
    ///  gets the jwt token as string or prompts the user to login
    /// </summary>
    /// <returns>jwt token as string</returns>
    public string GetToken()
    {
        return _token != null ? _token.RawData : NewToken().RawData;
    }

    /// <summary>
    /// gets the ani list userId or prompts the user to login
    /// </summary>
    /// <returns></returns>
    public string GetUserId()
    {
        return _userId ?? NewToken().Subject;
    }
    
    /// <summary>
    ///  opens a browser to the  aniList login page and asks the user to enter the displayed token and then tests it;
    ///  should this fail the user is asked if they want to try again or abort. 
    /// </summary>
    /// <exception cref="AuthenticationException">jwt token was not correct and user didn't retry.</exception>
    /// <returns>Jwt token as string</returns>
    public JwtSecurityToken NewToken()
    {
        string url = _parameter.ApiAuthEndpoint!;
        try
        {
            Process.Start(url);
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
        
        string token = AnsiConsole.Prompt(new TextPrompt<string>("Please paste the token:"));

        try
        {
            _token = new JwtSecurityToken(token);
            _userId = _token.Subject;
            if (_parameter.SaveAuthToken)
            {
                AuthToken authToken = new AuthToken(_token);
                authToken.Save();
            }
            return _token;
        }
        catch (Exception exception)
        {
            bool result = AnsiConsole.Prompt(
                new SelectionPrompt<bool>()
                    .Title("[red bold]Error:[/] the token you entered is not correct would you like to try again?")
                    .AddChoices(true,false)
                );
            if (result)
            {
                return NewToken();
            }

            throw new AuthenticationException("jwt token was not correct and user didn't retry.",exception);
        }
    }
    
}