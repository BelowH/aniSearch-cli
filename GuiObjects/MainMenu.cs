
using System.Diagnostics;
using System.Runtime.InteropServices;
using Spectre.Console;

namespace aniList_cli.GuiObjects;

public class MainMenu : IMainMenu
{
    
    private readonly ISearchPage _searchPage;
    
    public MainMenu( ISearchPage searchPage)
    {
        _searchPage = searchPage;
        _searchPage.OnBackToMenu += (_,_) => Display();
    }

    private const string SSearch = "Search";
    private const string SMyList = "My List";
    private const string SLogin = "Login";
    private const string SProfile = "Profile";
    private const string SSettings = "Settings";
    private const string SExit = "[red]Exit[/]";
    

    public void Display()
    {
        try
        {
            Console.Clear();
            Rule title = new Rule("[bold blue]Welcome to aniList-cli![/]");
            title.Alignment = Justify.Left;
            AnsiConsole.Write(title);
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new []
                    {
                        SSearch,
                        SMyList,
                        SProfile,
                        SLogin,
                        SSettings,
                        SExit
                    })
            );
            switch (choice)
            {
                case SSearch :
                    Search();
                    break;
                case SMyList:
                    My_List();
                    break;
                case SProfile:
                    Profile();
                    break;
                case SLogin:
                    Login();
                    break;
                case SSettings:
                    Settings();
                    break;
                case SExit:
                    Exit();
                    break;
                default:
                    //should not happen
                    throw new ArgumentException("choice not found");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private void Search()
    {
        _searchPage.Display();
    }

    private void My_List()
    {
        Console.WriteLine("Not implemented yet :(");
        Display(); 
    }
    
    private void Login()
    {
        string url = "https://anilist.co/api/v2/oauth/authorize?client_id=8875&response_type=token";
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
        Console.ReadKey();
        Console.WriteLine("Not implemented yet :(");
        Display();
    }

    private void Profile()
    {
        Console.WriteLine("Not implemented yet :(");
        Display();
    }

    private void Settings()
    {
        Console.WriteLine("Not implemented yet :(");
        Display();
    }

    private void Exit()
    {
        Console.Clear();
        Environment.Exit(0);
    }
}