using CoreGraphics;
using NotepadGps.Controls;
using NotepadGps.iOS.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomBottomTabbedPage), typeof(IOSCustomBottomTabbedPageRender))]
namespace NotepadGps.iOS.Renders
{
    class IOSCustomBottomTabbedPageRender : TabbedRenderer
    {
        public UIImage imageWithColor(CGSize size)
        {
            CGRect rect = new CGRect(0, 0, size.Width, size.Height);
            UIGraphics.BeginImageContext(size);

            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(UIColor.LightGray.CGColor);
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
    }
}