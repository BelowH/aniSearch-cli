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
            Back();
        }
        catch (Exception e)
        {
            AnsiConsole.Markup("[red]Error:[/] Some error occurred while trying to fetch user data." + Environment.NewLine + e.Message+ Environment.NewLine + "Press any key to continue.");
            Console.ReadKey();
            Back();
        }
        Console.Clear();
        Rule rule = new Rule("[bold blue]" + user.Name + "[/]");
        rule.Alignment = Justify.Center;
        rule.Border = BoxBorder.Rounded;
        AnsiConsole.Write(rule);

        if (user.StatisticTypes != null )
        {
            if (user.StatisticTypes.AnimeStatistics != null)
            {
                UserStatistics animeStats = user.StatisticTypes.AnimeStatistics;
                Table animeTable = new Table();
                animeTable.Alignment = Justify.Center;
                animeTable.Border = TableBorder.Rounded;
                animeTable.AddColumn("Total Anime");
                animeTable.AddColumn("Days Watched");
                animeTable.AddColumn("Mean Score");
                animeTable.AddRow($"[blue]{animeStats.Count}[/]",$"[blue]{animeStats.MinutesWatched/1440}[/]",$"[blue]{animeStats.MeanScore}[/]");
                AnsiConsole.Write(animeTable);

                if (animeStats.Statistic is { Length: > 0 })
                {
                    BarChart animeChart = new BarChart().Width(animeStats.Count);
                    foreach (UserStatusStatistic statistic in animeStats.Statistic)
                    {
                        animeChart.AddItem(statistic.MediaListStatus.ToString(), statistic.Count, Color.Blue);
                    }
                    AnsiConsole.Write(animeChart);
                }
                
            }
            if (user.StatisticTypes.MangaStatistics != null)
            {
                UserStatistics mangaStats = user.StatisticTypes.MangaStatistics;
                Table mangaTable = new Table();
                mangaTable.Alignment = Justify.Center;
                mangaTable.Border = TableBorder.Rounded;
                mangaTable.AddColumn("Total Manga");
                mangaTable.AddColumn("Chapters Read");
                mangaTable.AddColumn("Volumes Read");
                mangaTable.AddRow($"[blue]{mangaStats.Count}[/]",$"[blue]{mangaStats.ChaptersRead}[/]",$"[blue]{mangaStats.VolumesRead}[/]");
                AnsiConsole.Write(mangaTable);
                
                if (mangaStats.Statistic is { Length: > 0 })
                {
                    BarChart animeChart = new BarChart().Width(mangaStats.Count);
                    foreach (UserStatusStatistic statistic in mangaStats.Statistic)
                    {
                        animeChart.AddItem(statistic.MediaListStatus.ToString(), statistic.Count, Color.Blue);
                    }
                    AnsiConsole.Write(animeChart);
                }
            }
            
        }
        
        AnsiConsole.Markup("[red](R)eturn to menu[/]");
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key != ConsoleKey.R) continue;
            Back();
            return;
        }
    }

    private void Back()
    {
        EventHandler? handler = OnBackToMenu;
        handler?.Invoke(this, EventArgs.Empty);
    }
}