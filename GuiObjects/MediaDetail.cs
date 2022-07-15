using aniList_cli.Repository;
using aniList_cli.Repository.Models;
using Spectre.Console;

namespace aniList_cli.GuiObjects;

public class MediaDetail : IPage
{
    private readonly int _mediaId;

    private readonly SearchRepository _searchRepository;
    
    public MediaDetail(int mediaId)
    {
        _searchRepository = new SearchRepository();
        _mediaId = mediaId;
    }
    
    public void Display()
    {
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
        throw new NotImplementedException();
    }
}