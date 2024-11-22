using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine;
using Windows.ApplicationModel.Activation;
using Riverside.Runtime.Modern;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;

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
            var rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;

            if (arguments != null)
            {
                // Handle command line arguments
                var rootCommand = new RootCommand
                {
                    new Option<string>("--example", "An example option")
                };

                rootCommand.Handler = CommandHandler.Create<string>((example) =>
                {
                    // Handle the example option
                    Console.WriteLine($"Example option: {example}");
                });

                rootCommand.Invoke(arguments);
            }

            var mainWindow = new Window();
            mainWindow.Content = rootFrame;
            mainWindow.Activate();

            windows["Main"] = mainWindow;
        }

        public static void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
