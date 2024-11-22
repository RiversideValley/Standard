using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace Riverside.Runtime.Modern.Helpers
{
    public static class Activation
    {
        public static void HandleProtocolActivation(Uri uri)
        {
            // Handle protocol activation
            Console.WriteLine($"Protocol activated: {uri}");
        }

        public static void HandleActivation(string arguments, Dictionary<string, Window> windows)
        {
            Frame rootFrame = new();
            rootFrame.NavigationFailed += OnNavigationFailed;

            if (arguments != null)
            {
                // Handle command line arguments
                RootCommand rootCommand =
                [
                    new Option<string>("--example", "An example option")
                ];

                rootCommand.Handler = CommandHandler.Create<string>((example) =>
                {
                    // Handle the example option
                    Console.WriteLine($"Example option: {example}");
                });

                _ = rootCommand.Invoke(arguments);
            }

            Window mainWindow = new()
            {
                Content = rootFrame
            };
            mainWindow.Activate();

            windows["Main"] = mainWindow;
        }

        public static void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
