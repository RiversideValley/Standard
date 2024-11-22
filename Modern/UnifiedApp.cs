using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Riverside.Runtime.Modern.Helpers;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel;

// Excuse half of this code being copied directly from Files and FluentHub :D

namespace Riverside.Runtime.Modern
{
    public partial class UnifiedApp : Application
    {
        public Dictionary<string, Window> _windows = [];
        public new static UnifiedApp Current
            => (UnifiedApp)UnifiedApp.Current;

        public readonly static string AppVersion =
            $"{Package.Current.Id.Version.Major}." +
            $"{Package.Current.Id.Version.Minor}." +
            $"{Package.Current.Id.Version.Build}." +
            $"{Package.Current.Id.Version.Revision}";

        public UnifiedApp()
        {
            // this.InitializeComponent();
            // UnhandledException += OnUnhandledException;
            // this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Activation.HandleActivation(args.Arguments, _windows);
        }

        protected void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                Activation.HandleProtocolActivation(protocolArgs.Uri);
            }
        }

        public void OpenNewWindow(string windowKey, Type pageType, object parameter = null)
        {
            if (_windows.TryGetValue(windowKey, out Window value))
            {
                value.Activate();
                return;
            }

            var newWindow = new Window();
            var frame = new Frame();
            frame.Navigate(pageType, parameter);
            newWindow.Content = frame;
            newWindow.Activate();

            _windows[windowKey] = newWindow;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
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
