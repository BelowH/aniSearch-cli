using System.Reflection;
using aniList_cli.GuiObjects;
using Spectre.Console;

namespace aniList_cli;

static class Program{

    public static void Main(string[] args)
    {
       
        MainMenu menu = new MainMenu();
        menu.Display();

        Console.ReadKey();
    }

}