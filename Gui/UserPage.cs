using System.Security.Authentication;
using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using aniList_cli.Service;
using aniList_cli.Settings;
using Spectre.Console;

namespace aniList_cli.Gui;

public class UserPage : IUserPage
{

    private readonly AppParameter _parameter;

    private readonly ILoginService _loginService;

    private readonly IUserRepository _userRepository;

    public UserPage(AppParameter parameter, ILoginService loginService, IUserRepository userRepository)
    {
        _parameter = parameter;
        _loginService = loginService;
        _userRepository = userRepository;
    }

    public event EventHandler? OnBackToMenu;

    /// <summary>
    ///  Gets userId from service, then loads user data and displays it.
    ///  Should something go wrong a Message is Displayed and the
    /// </summary>
    public void Display()
    {
        int userId = 0;
        AniListUser user = new AniListUser(); 
        
        try
        {
            userId = int.Parse(_loginService.GetUserId());
            Console.Clear();
            AnsiConsole.Status().Start(
                "Loading User",
                ctx =>
                {
                    ctx.SpinnerStyle = new Style(Color.Blue);
                    user = _userRepository.GetUserById(userId).Result!;
                }
            );

        }
        catch (AuthenticationException)
        {
            //Case jwt token was not valid and user won't retry
            OnBack();
        }
        catch (Exception e)
        {
            AnsiConsole.Markup("[red]Error:[/] Some error occurred while trying to fetch user data." + Environment.NewLine + e.Message+ Environment.NewLine + "Press any key to continue.");
            Console.ReadKey();
            OnBack();
        }

        Rule rule = new Rule("[bold blue]" + user.Name + "[/]");
        rule.Alignment = Justify.Center;
        rule.Border = BoxBorder.Rounded;
        AnsiConsole.Write(rule);


    }

    private void OnBack()
    {
        EventHandler? handler = OnBackToMenu;
        handler?.Invoke(this, EventArgs.Empty);
    }
}