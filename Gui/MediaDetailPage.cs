using aniList_cli.Repository.Models;
using aniList_cli.Repository.UnauthenticatedRequests;
using Spectre.Console;

namespace aniList_cli.Gui;

public class MediaDetailPage : IMediaDetailPage
{
    private int _mediaId;

    private readonly ISearchRepository _searchRepository;
    
    public event EventHandler? OnBack;
    
    public MediaDetailPage( ISearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
    public void Display(int id, bool isInList = false)
    {
        _mediaId = id;
        Console.Clear();
        Media media = new Media();
        AnsiConsole.Status().Start(
            "Loading Media",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                media = _searchRepository.SearchById(_mediaId).Result;
            }
        );

        Rule rule = new Rule("[bold blue]"+media.Title + "[/]");
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
        seasonTable.AddRow((media.Season.ToString() ?? "-") + " " + (media.SeasonYear.ToString() ?? "-"));

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
        studioTable.AddRow(studio);
        
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
        descriptionTable.AddRow(media.Description ?? "unknown");
        
        AnsiConsole.Write(descriptionTable);
        
        AnsiConsole.MarkupLine("[red](R)eturn to Search[/] [green](A)dd to Watchlist[/]");
        
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.R:
                    Back();
                    return;
                case ConsoleKey.A:
                    AddToWatchlist();
                    break;
            }
        }
    }

    public void Back()
    {
        EventHandler? handler = OnBack;
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    public void AddToWatchlist()
    {
        
    }
}