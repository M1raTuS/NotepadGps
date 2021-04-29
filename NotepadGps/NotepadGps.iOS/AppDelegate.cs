﻿using CarouselView.FormsPlugin.iOS;
using Foundation;
using UIKit;

namespace NotepadGps.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();

            Xamarin.FormsGoogleMaps.Init("AIzaSyAf7rljVibHW7F1neBavieHa9UZ5t3Co5s");

            LoadApplication(new App());

            CarouselViewRenderer.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
