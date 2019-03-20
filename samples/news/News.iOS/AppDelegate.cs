﻿using Foundation;
using UIKit;

namespace News.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

#if DEBUG
            Xamarin.Calabash.Start();
#endif

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            Lottie.Forms.iOS.Renderers.AnimationViewRenderer.Init();
            News.Bootstrap.Begin();

            return base.FinishedLaunching(app, options);
        }
    }
}