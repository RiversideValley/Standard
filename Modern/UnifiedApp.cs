using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Riverside.Runtime.Modern.Helpers;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Riverside.Runtime.Modern
{
    /// <summary>
    /// Represents the application class that provides multiple windowing support,
    /// protocol activation handling, and command line integration.
    /// </summary>
    public partial class UnifiedApp : Application
    {
        /// <summary>
        /// Dictionary to keep track of open windows.
        /// </summary>
        public Dictionary<string, Window> _windows = [];

        /// <summary>
        /// Gets the current instance of the <see cref="UnifiedApp"/>.
        /// </summary>
        public static new UnifiedApp Current => (UnifiedApp)Application.Current;

        /// <summary>
        /// Gets the application version.
        /// </summary>
        public static readonly string AppVersion =
            $"{Package.Current.Id.Version.Major}." +
            $"{Package.Current.Id.Version.Minor}." +
            $"{Package.Current.Id.Version.Build}." +
            $"{Package.Current.Id.Version.Revision}";

        /// <summary>
        /// Initializes a new instance of the <see cref="UnifiedApp"/> class.
        /// </summary>
        public UnifiedApp()
        {
            // this.InitializeComponent();
            // UnhandledException += OnUnhandledException;
            // this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Activation.HandleActivation(args.Arguments, _windows);
        }

        /// <summary>
        /// Invoked when the application is activated.
        /// </summary>
        /// <param name="args">Details about the activation request and process.</param>
        protected void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;
                Activation.HandleProtocolActivation(protocolArgs.Uri);
            }
        }

        /// <summary>
        /// Opens a new window with the specified key, page type, and optional parameter.
        /// </summary>
        /// <param name="windowKey">The key to identify the window.</param>
        /// <param name="pageType">The type of the page to navigate to.</param>
        /// <param name="parameter">The parameter to pass to the page.</param>
        public void OpenNewWindow(string windowKey, Type pageType, object parameter = null)
        {
            if (_windows.TryGetValue(windowKey, out Window value))
            {
                value.Activate();
                return;
            }

            Window newWindow = new();
            Frame frame = new();
            _ = frame.Navigate(pageType, parameter);
            newWindow.Content = frame;
            newWindow.Activate();

            _windows[windowKey] = newWindow;
        }

        /// <summary>
        /// Invoked when the application execution is being suspended.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        //private async void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        //    => await AppUnhandledException(e.Exception);

        //private async Task AppUnhandledException(Exception ex)
        //{
        //    Ioc.Default.GetService<Utils.ILogger>()?.Fatal("Unhandled exception", ex);

        //    try
        //    {
        //        await new Microsoft.UI.Xaml.Controls.ContentDialog
        //        {
        //            Title = "Unhandled exception",
        //            Content = ex.Message,
        //            CloseButtonText = "Close"
        //        }
        //        .ShowAsync();
        //    }
        //    catch (Exception ex2)
        //    {
        //        Ioc.Default.GetService<Utils.ILogger>()?.Error("Failed to display unhandled exception", ex2);
        //    }
        //}
    }
}
