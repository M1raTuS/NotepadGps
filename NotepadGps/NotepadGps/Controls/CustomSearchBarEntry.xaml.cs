using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.Controls
{
    public partial class CustomSearchBarEntry : Grid
    {
        public CustomSearchBarEntry()
        {
            InitializeComponent();
            img.IsVisible = false;
        }

        #region -- Public properties --

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                               propertyName: nameof(Text),
                                                               returnType: typeof(string),
                                                               declaringType: typeof(CustomSearchBarEntry),
                                                               defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                               propertyName: nameof(Placeholder),
                                                               returnType: typeof(string),
                                                               declaringType: typeof(CustomSearchBarEntry));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
                                                               propertyName: nameof(PlaceholderColor),
                                                               returnType: typeof(string),
                                                               declaringType: typeof(CustomSearchBarEntry));

        public string PlaceholderColor
        {
            get => (string)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty LblTextProperty = BindableProperty.Create(
                                                            propertyName: nameof(LblText),
                                                            returnType: typeof(string),
                                                            declaringType: typeof(CustomSearchBarEntry));

        public string LblText
        {
            get => (string)GetValue(LblTextProperty);
            set => SetValue(LblTextProperty, value);
        }

        public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create(
                                                                       propertyName: nameof(ErrorMessage),
                                                                       returnType: typeof(string),
                                                                       declaringType: typeof(CustomSearchBarEntry));

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public static readonly BindableProperty CancelButtonProperty = BindableProperty.Create(
                                                                       propertyName: nameof(CancelButton),
                                                                       returnType: typeof(bool),
                                                                       declaringType: typeof(CustomSearchBarEntry));

        public bool CancelButton
        {
            get => (bool)GetValue(CancelButtonProperty);
            set => SetValue(CancelButtonProperty, value);
        }

        public static readonly BindableProperty FrameBorderColorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(FrameBorderColor),
                                                                           returnType: typeof(Color),
                                                                           declaringType: typeof(CustomSearchBarEntry));

        public Color FrameBorderColor
        {
            get => (Color)GetValue(FrameBorderColorProperty);
            set => SetValue(FrameBorderColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
                                                                    propertyName: nameof(TextColor),
                                                                    returnType: typeof(Color),
                                                                    declaringType: typeof(CustomSearchBarEntry));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public ICommand TapGestureRecognizer => new Command(OnTapGestureRecognizer);

        private void OnTapGestureRecognizer(object obj)
        {
            custom.Text = string.Empty;
        }

        #endregion

        #region -- Overrides -- 

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Text))
            {
                if (string.IsNullOrEmpty(Text))
                {
                    img.IsVisible = false;
                }
                else
                {
                    img.IsVisible = true;
                }
            }
        }

        #endregion
    }
}