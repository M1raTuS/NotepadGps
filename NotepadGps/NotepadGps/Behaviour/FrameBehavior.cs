using Xamarin.Forms;

namespace NotepadGps.Behaviour
{
    public class FrameBehavior : Frame
    {
        public bool ShowFrame
        {
            get => (bool)GetValue(ShowFrameProperty);

            set => SetValue(ShowFrameProperty, value);
        }
        public static readonly BindableProperty ShowFrameProperty = BindableProperty.Create(
                                                         propertyName: nameof(ShowFrame),
                                                         returnType: typeof(bool),
                                                         declaringType: typeof(FrameBehavior),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: ShowFramePropertyChanged);


        private static void ShowFramePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var frame = bindable as FrameBehavior;
            var isValid = (bool)newValue;

            if (frame != null)
            {
                if (isValid)
                {
                    frame.TranslateTo(0, -130, 400);
                }
                else
                {
                    frame.TranslateTo(0, 10, 500);
                }
            }

        }
    }
}
