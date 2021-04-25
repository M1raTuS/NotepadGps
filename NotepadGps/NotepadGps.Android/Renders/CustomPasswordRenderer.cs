using Android.Content;
using NotepadGps.Controls;
using NotepadGps.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StandartEntry), typeof(CustomStandardEntryRenderer))]
namespace NotepadGps.Droid.Renders
{
    public class CustomPasswordRenderer : EntryRenderer
    {
        public CustomPasswordRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}