using CoreGraphics;
using NotepadGps.Controls;
using NotepadGps.iOS.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTopTabbedPage), typeof(IOSCustomTopTabbedPageRender))]
namespace NotepadGps.iOS.Renders
{
    class IOSCustomTopTabbedPageRender : TabbedRenderer
    {
        public UIImage imageWithColor(CGSize size)
        {
            CGRect rect = new CGRect(0, 0, size.Width, size.Height);
            UIGraphics.BeginImageContext(size);

            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(UIColor.SystemFillColor.CGColor);
                context.FillRect(rect);
            }

            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            CGSize size = new CGSize(TabBar.Frame.Width / TabBar.Items.Length, TabBar.Frame.Height);

            UITabBar.Appearance.SelectionIndicatorImage = imageWithColor(size);
            UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
            UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.Black }, UIControlState.Selected);
        }

        //public override void ViewWillLayoutSubviews()
        //{
        //    base.ViewWillLayoutSubviews();

        //    nfloat tabSize = 44.0f;

        //    UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;

        //    if (UIInterfaceOrientation.LandscapeLeft == orientation || UIInterfaceOrientation.LandscapeRight == orientation)
        //    {
        //        tabSize = 32.0f;
        //    }

        //    CGRect rect = this.View.Frame;
        //    rect.Y = this.NavigationController != null ? tabSize : tabSize + 20;
        //    this.View.Frame = rect;

        //    if (TabBarController != null)
        //    {
        //        CGRect tabFrame = this.TabBarController.TabBar.Frame;
        //        tabFrame.Height = tabSize;
        //        tabFrame.Y = this.NavigationController != null ? 64 : 20;
        //        this.TabBarController.TabBar.Frame = tabFrame;
        //    }
        //}
    }
}