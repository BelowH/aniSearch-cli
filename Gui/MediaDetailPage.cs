using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using aniList_cli.Service;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MediaDetailPage : IMediaDetailPage
{
    private int _mediaId;

    private MediaListStatus? _userStatus;

    private int _progress;

    private readonly IUnAuthenticatedQueries _unAuthenticatedQueries;

    private readonly IAuthenticatedQueries _authenticatedQueries;

    private readonly IMutationPage _mutationPage;

    private readonly ILoginService _loginService;

    private IMainMenu.Callback? _caller;
    
    public MediaDetailPage( IUnAuthenticatedQueries unAuthenticatedQueries, ILoginService loginService, IAuthenticatedQueries authenticatedQueries, IMutationPage mutationPage)
    {
        _unAuthenticatedQueries = unAuthenticatedQueries;
        _loginService = loginService;
        _authenticatedQueries = authenticatedQueries;
        _mutationPage = mutationPage;
        _mutationPage.OnBack += (_, _) => DisplayMedia(_mediaId, _caller);
    }
    
    public void DisplayMedia(int id, IMainMenu.Callback? caller)
    {
        _mediaId = id;
        _caller = caller;
        MediaStatusInfo? mediaStatusInfo = null;
        Console.Clear();
        Media? media = new Media();
        AnsiConsole.Status().Start(
            "Loading Media",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                media = _unAuthenticatedQueries.SearchById(_mediaId);
            }
        );
        AnsiConsole.Status().Start(
            "Loading Status",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                if (_loginService.IsUserLoggedIn())
                {
                    mediaStatusInfo = _authenticatedQueries.GetMediaStatusByMediaId(_mediaId);
                } 
            }
        );
        Console.Clear();
        if (media == null)
        {
            AnsiConsole.MarkupLine("[red bold]No Media found.[/]" + Environment.NewLine + "Press any key to go back.");
            Console.ReadKey();
            _caller?.Invoke();
            return;
        }
        if (mediaStatusInfo != null)
        {
            _userStatus = mediaStatusInfo.Status;
            _progress = mediaStatusInfo.Progress ?? 0;
        }
        
        Rule rule = new Rule("[bold blue]"+Markup.Escape(media.Title.ToString()) + "[/]");
        rule.Alignment = Justify.Center;
        rule.Border = BoxBorder.Rounded;
        AnsiConsole.Write(rule);
        
        Table statusTable = new Table();
        statusTable.Alignment = Justify.Center;
        statusTable.Border = TableBorder.Rounded;
        
        statusTable.AddColumn("Type");
        statusTable.AddColumn("Format");
        statusTable.AddColumn("Status");
        
        statusTable.AddRow(
            media.Type.ToString(), 
            media.Format.ToString() ?? "unknown",
            media.Status.ToString() ?? "unknown");

        Table lengthAndScoreTable = new Table();
        lengthAndScoreTable.Alignment = Justify.Center;
        lengthAndScoreTable.Border = TableBorder.Rounded;
        lengthAndScoreTable.AddColumn(media.Type == MediaType.ANIME ? "Episodes" : "Chapters");
        lengthAndScoreTable.AddColumn("Score");
        
        string mediaLength = media.Type == MediaType.ANIME ? 
            media.Episodes.ToString() ?? "-" : media.Chapters.ToString() ?? "-";
        lengthAndScoreTable.AddRow(mediaLength,media.AverageScore.ToString() ?? "-");

        Table seasonTable = new Table();
        seasonTable.Alignment = Justify.Center;
        seasonTable.Border = TableBorder.Rounded;
        seasonTable.AddColumn("Season");
        seasonTable.AddRow(Markup.Escape((media.Season.ToString() ?? "-") + " " + (media.SeasonYear.ToString() ?? "-")));

        Table studioTable = new Table();
        studioTable.Alignment = Justify.Center;
        studioTable.Border = TableBorder.Rounded;
        studioTable.AddColumn("Studio");
        string studio = "";
        if (media.Studio is  { Nodes.Length: > 0 })
        {
            foreach (MediaStudio.Node node in media.Studio.Nodes)
            {
                if (node.IsAnimationStudio)
                {
                    studio += node.Name + ", ";
                }
            }
        }
        else
        {
            studio = "-";
        }
        studioTable.AddRow(Markup.Escape(studio));
        
        Table headTable = new Table();
        headTable.Alignment = Justify.Center;
        headTable.Border = TableBorder.None;
        headTable.AddColumn(new TableColumn(""));
        headTable.AddColumn(new TableColumn(""));
        headTable.AddColumn(new TableColumn(""));
        headTable.AddColumn(new TableColumn(""));
        headTable.AddRow(statusTable,lengthAndScoreTable,seasonTable,studioTable);
        
        AnsiConsole.Write(headTable);

        Table descriptionTable = new Table();
        descriptionTable.Alignment = Justify.Center;
        descriptionTable.Border = TableBorder.Rounded;

        descriptionTable.AddColumn("Description:");
        descriptionTable.AddRow(Markup.Escape(media.Description ?? "unknown"));
        
        AnsiConsole.Write(descriptionTable);

        string controls = "[red](R)eturn to Search[/] ";
        if (mediaStatusInfo != null)
        {
            Table listTable = new Table();
            listTable.HideHeaders();
            listTable.Alignment = Justify.Center;
            listTable.Border = TableBorder.Rounded;
            listTable.AddColumn(new TableColumn(""));
            if (media.Type == MediaType.ANIME )
            {
                listTable.AddRow("Progress: " + _progress + " out of " + (media.Episodes ?? 0));
            }else 
            {
                listTable.AddRow("Progress: " + _progress + " out of " + (media.Chapters ?? 0));
            }
            listTable.AddRow("In List: " + _userStatus);
            AnsiConsole.Write(listTable);
            if (media.Type == MediaType.ANIME)
            {
                controls += "[green](A)dd Episode[/] ";
            }
            else
            {
                controls += "[green](A)dd Chapter[/] ";
                controls += "[green]add (V)olume[/] ";
            }
            controls += "[yellow](M)ove to List[/]";
        }
        else
        {
            controls += "[green](A)dd to List[/]";
        }
        AnsiConsole.MarkupLine(controls);
        
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.R:
                    _caller?.Invoke();
                    return;
            }

            if (mediaStatusInfo != null)
            {
                switch (key.Key)
                {
                    case ConsoleKey.A:
                        _mutationPage.AddProgress(media,mediaStatusInfo);
                        DisplayMedia(_mediaId, _caller);
                        break;
                    case ConsoleKey.V:
                        if (media.Type == MediaType.MANGA)
                        {
                            _mutationPage.AddProgress(media,mediaStatusInfo,true);
                            DisplayMedia(_mediaId, _caller);
                        }
                        break;
                    case ConsoleKey.M:
                        _mutationPage.MoveToList(media.Id, mediaStatusInfo);
                        DisplayMedia(_mediaId, _caller);
                        break;
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKey.A:
                        _mutationPage.MoveToList(media.Id);
                        break;
                }
            }
        }
    }
}