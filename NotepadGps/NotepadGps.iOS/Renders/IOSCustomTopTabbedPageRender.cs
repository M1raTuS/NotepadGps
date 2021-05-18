using NotepadGps.Controls;
using NotepadGps.iOS.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTopTabbedPage), typeof(IOSCustomTopTabbedPageRender))]
namespace NotepadGps.iOS.Renders
{
    class IOSCustomTopTabbedPageRender : TabbedRenderer
    {
    }
}