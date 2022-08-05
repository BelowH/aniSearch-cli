﻿using System.Reflection;
using aniList_cli.Gui;
using aniList_cli.Helper;
using aniList_cli.Repository;
using aniList_cli.Repository.AuthenticatedRequests;
using aniList_cli.Repository.UnauthenticatedRequests;
using aniList_cli.Service;
using aniList_cli.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

namespace aniList_cli;

static class Program
{
    
    public static async Task Main(string[] args)
    {
        
        try
        {
            //Setup DI
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => ConfigureServices(services))
                .Build();

            //end program when user hist Ctrl + c
            Console.CancelKeyPress += async (_,_) =>
            {
                await host.StopAsync(new TimeSpan(100));
                Console.CursorVisible = true;
                Console.Clear();
                Environment.Exit(1);
            };
            using (host)
            {
                await host.StartAsync();
                host.Services.GetService<IMainMenu>()!.Display();
            }
        }
        catch (Exception e)
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name!;
            string version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            Console.WriteLine("Sorry, an exception occurred while running: {0}" + Environment.NewLine 
                + "Version: {1}" + Environment.NewLine + "Exception: {2}" , name, version, e.Message );
            Console.WriteLine("Press any key to end ...");
            Console.ReadKey();
        }
    }

    /// <summary>
    ///  And load settings
    ///  Configure Services for DI
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureServices(IServiceCollection services)
    {
        //load and register app parameter
        AppParameter parameter = FileHelper.ReadAppParameter();
        services.AddSingleton(parameter);

        //register services
        services.AddTransient<IAuthenticatedQueries, AuthenticatedQueries>();
        services.AddTransient<IMutationPage, MutationPage>();
        services.AddTransient<IMediaListPage, MediaListPage>();
        services.AddSingleton<ILoginService, LoginService>();
        services.AddTransient<IUserPage, UserPage>();
        services.AddTransient<IUnAuthenticatedQueries,UnAuthenticatedQueries>();
        services.AddTransient<IMediaDetailPage, MediaDetailPage>();
        services.AddTransient<ISearchPage, SearchPage>();
        services.AddTransient<IMainMenu, MainMenu>();
        
    }
    
    
    
    

}