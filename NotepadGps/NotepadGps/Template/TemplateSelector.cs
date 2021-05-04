using NotepadGps.View;
using NotepadGps.ViewModel;
using Xamarin.Forms;

namespace NotepadGps.Template
{
    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddEditPinViews { get; set; }
        public DataTemplate EventViews { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is AddEditMapPinViewModel)
            {
                return AddEditPinViews;
            }

            if (item is EventsViewModel)
            {
                return EventViews;
            }

            return null;
        }
    }
}
