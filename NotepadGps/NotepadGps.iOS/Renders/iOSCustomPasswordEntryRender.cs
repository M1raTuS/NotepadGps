using NotepadGps.Controls;
using NotepadGps.iOS.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StandartEntry), typeof(iOSCustomPasswordEntryRender))]
namespace NotepadGps.iOS.Renders
{
    class iOSCustomPasswordEntryRender : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
        }
    }
}