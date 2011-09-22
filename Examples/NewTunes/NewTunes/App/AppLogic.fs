namespace NewTunes

open System
open System.Net
open System.Windows
open System.Windows.Controls
open System.Windows.Documents
open System.Windows.Ink
open System.Windows.Input
open System.Windows.Media
open System.Windows.Media.Animation
open System.Windows.Shapes
open System.Windows.Navigation
open Microsoft.Phone.Controls
open Microsoft.Phone.Shell
open Caliburn.Micro



/// This type implements the main page of the application
type MainPage() as this =
    inherit PhoneApplicationPage()
    // Load the Xaml for the page.
    do Application.LoadComponent(this, new System.Uri("/NewTunes;component/MainPage.xaml", System.UriKind.Relative))

/// One instance of this type is created in the application host project.
type App(app:Application) = 
    // Global handler for uncaught exceptions. 
    // Note that exceptions thrown by ApplicationBarItem.Click will not get caught here.
    do app.UnhandledException.Add(fun e -> 
            if (System.Diagnostics.Debugger.IsAttached) then
                // An unhandled exception has occurred, break into the debugger
                System.Diagnostics.Debugger.Break();
     )

    let navigationService = IoC.Get<INavigationService>()
    do navigationService.Navigate(new Uri("/NewTunes;component/MainPage.xaml", UriKind.Relative)) |> ignore
