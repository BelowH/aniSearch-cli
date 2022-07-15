using aniList_cli.Repository;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.GuiObjects;

public class MediaDetail : IMediaDetail
{
    private int _mediaId;

    private readonly ISearchRepository _searchRepository;
    
    public event EventHandler? OnBack;
    
    public MediaDetail( ISearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
    public void Display(int id)
    {
        _mediaId = id;
        Console.Clear();
        Media media = new Media();
        AnsiConsole.Status().Start(
            "Loading",
            ctx =>
            {
                ctx.SpinnerStyle = new Style(Color.Blue);
                media = _searchRepository.SearchById(_mediaId).Result;
            }
        );

        Table table = new Table();
        table.Title = new TableTitle("[bold blue]"+media.Title + "[/]");
        
        table.AddColumn(new TableColumn("Description:"));
        table.AddColumn(new TableColumn(media.Description ?? "unknown"));
        
        AnsiConsole.Write(table);
    }

    public void Back()
    {
        EventHandler? handler = OnBack;
        handler?.Invoke(this, EventArgs.Empty);
    }
}