using NotepadGps.Controls;
using NotepadGps.iOS.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StandartEntry), typeof(iOSStandardEntryRender))]
namespace NotepadGps.iOS.Renders
{
    public class iOSStandardEntryRender : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
        }
    }
}