using System.Security.Authentication;
using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using aniList_cli.Service;
using Spectre.Console;

namespace aniList_cli.Gui;

public class UserPage : IUserPage
{
    private readonly ILoginService _loginService;
    private readonly IUnAuthenticatedQueries _repository;

    public UserPage(ILoginService loginService, IUnAuthenticatedQueries repository)
    {
        _loginService = loginService;
        _repository = repository;
    }

    /// <summary>
    ///  Gets userId from service, then loads user data and displays it.
    ///  Should something go wrong a Message is Displayed and the user can return to the menu.
    /// </summary>
    public void Display()
    {
        int userId;
        AniListUser? user = null; 
        
        try
        {
            userId = int.Parse(_loginService.GetUserId());
            AnsiConsole.Status().Start(
                "Loading User",
                ctx =>
                {
                    ctx.SpinnerStyle = new Style(Color.Blue);
                    user = _repository.GetUserById(userId)!;
                }
            );

        }
        catch (AuthenticationException)
        {
            //Case jwt token was not valid and user won't retry
            return;
        }
        catch (Exception e)
        {
            AnsiConsole.Markup("[red]Error:[/] Some error occurred while trying to fetch user data." + Environment.NewLine + e.Message+ Environment.NewLine + "Press any key to continue.");
            Console.ReadKey();
        }

        if (user == null)
        {
            AnsiConsole.MarkupLine("[red bold]No Media found.[/]\nPress any key to go back.");
            Console.ReadKey();
            return;
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
                Table animeTable = new Table
                {
                    Title = new TableTitle("[blue bold]ANIME[/]"),
                    Border = TableBorder.None,
                    Alignment = Justify.Center,
                    
                };

                UserStatistics animeStats = user.StatisticTypes.AnimeStatistics;
                Table animeStatsTable = new Table
                {
                    Alignment = Justify.Center,
                    Border = TableBorder.Rounded,
                    Expand = false,
                    ShowHeaders = false
                };
                
                animeStatsTable.AddColumn("");
                animeStatsTable.AddColumn("");
                animeStatsTable.AddRow("Total Anime", $"[blue]{animeStats.Count}[/]");
                animeStatsTable.AddEmptyRow();
                animeStatsTable.AddRow("Days Watched",$"[blue]{Math.Round((decimal)animeStats.MinutesWatched/1440,2)}[/]");
                animeStatsTable.AddEmptyRow();
                animeStatsTable.AddRow("Mean Score",$"[blue]{animeStats.MeanScore}[/]");
                
                Table animeChartTable = new Table()
                {
                    Border = TableBorder.Rounded
                };
                if (animeStats.Statistic is { Length: > 0 })
                {
                    BarChart animeChart = new BarChart().Width(animeStats.Count);
                    foreach (UserStatusStatistic statistic in animeStats.Statistic)
                    {
                        animeChart.AddItem(statistic.MediaListStatus.ToString(), statistic.Count, Color.Blue);
                    }

                    animeChartTable.AddColumn(new TableColumn(animeChart));
                }
                else
                {
                    animeChartTable.AddColumn(new TableColumn("No Manga Statistics found."));
                }

                animeTable.AddColumn(new TableColumn(animeStatsTable));
                animeTable.AddColumn(new TableColumn(animeChartTable));
                AnsiConsole.Write(animeTable);
                
            }
            if (user.StatisticTypes.MangaStatistics != null)
            {
                Table mangaTable = new Table()
                {
                    Title = new TableTitle("[blue bold]MANGA[/]"),
                    Border = TableBorder.None,
                    Alignment = Justify.Center,
                };
                UserStatistics mangaStats = user.StatisticTypes.MangaStatistics;
                Table mangaStatsTable = new Table
                {
                    Alignment = Justify.Center,
                    Border = TableBorder.Rounded,
                    Expand = false,
                    ShowHeaders = false
                };
                mangaStatsTable.AddColumn("");
                mangaStatsTable.AddColumn("");
                mangaStatsTable.AddRow("Total Manga", $"[blue]{mangaStats.Count}[/]");
                mangaStatsTable.AddEmptyRow();
                mangaStatsTable.AddRow("Chapters Read", $"[blue]{mangaStats.ChaptersRead}[/]");
                mangaStatsTable.AddEmptyRow();
                mangaStatsTable.AddRow("Volumes Read", $"[blue]{mangaStats.VolumesRead}[/]");

                Table mangaChartTable = new Table()
                {
                    Border = TableBorder.Rounded,
                    Expand = true
                };
                if (mangaStats.Statistic is { Length: > 0 })
                {
                    BarChart mangaChart = new BarChart().Width(mangaStats.Count);
                    foreach (UserStatusStatistic statistic in mangaStats.Statistic)
                    {
                        mangaChart.AddItem(statistic.MediaListStatus.ToString(), statistic.Count, Color.Blue);
                    }

                    mangaChartTable.AddColumn(new TableColumn(mangaChart));
                }
                else
                {
                    mangaChartTable.AddColumn(new TableColumn("No Manga Statistics found."));
                }

                mangaTable.AddColumn(new TableColumn(mangaStatsTable));
                mangaTable.AddColumn(new TableColumn(mangaChartTable));
                AnsiConsole.Write(mangaTable);
            }
        }
        
        AnsiConsole.Markup("[red](R)eturn to menu[/]");
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.R) continue;
           
            return;
        }
    }
}