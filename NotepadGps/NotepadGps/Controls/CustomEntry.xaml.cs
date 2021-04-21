using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.Controls
{
    public partial class CustomEntry : Grid
    {
        public CustomEntry()
        {
            InitializeComponent(); 
            img.IsVisible = false;
        }

        #region -- Public properties --

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                               propertyName: nameof(Text),
                                                               returnType: typeof(string),
                                                               declaringType: typeof(CustomEntry),
                                                               defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                                      propertyName: nameof(Placeholder),
                                                                      returnType: typeof(string),
                                                                      declaringType: typeof(CustomEntry));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(PlaceholderColor),
                                                                           returnType: typeof(string),
                                                                           declaringType: typeof(CustomEntry));

        public string PlaceholderColor
        {
            get => (string)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty LblTextProperty = BindableProperty.Create(
                                                                  propertyName: nameof(LblText),
                                                                  returnType: typeof(string),
                                                                  declaringType: typeof(CustomEntry));

        public string LblText
        {
            get => (string)GetValue(LblTextProperty);
            set => SetValue(LblTextProperty, value);
        }

        public static readonly BindableProperty errorTextProperty = BindableProperty.Create(
                                                                    propertyName: nameof(errorText),
                                                                    returnType: typeof(string),
                                                                    declaringType: typeof(CustomEntry));

        public string errorText
        {
            get => (string)GetValue(errorTextProperty);
            set => SetValue(errorTextProperty, value);
        }

        public static readonly BindableProperty isErrorVisibleProperty = BindableProperty.Create(
                                                                   propertyName: nameof(isErrorVisible),
                                                                   returnType: typeof(bool),
                                                                   declaringType: typeof(CustomEntry));

        public bool isErrorVisible
        {
            get => (bool)GetValue(isErrorVisibleProperty);
            set => SetValue(isErrorVisibleProperty, value);
        }

        public static readonly BindableProperty CancelButtonProperty = BindableProperty.Create(
            propertyName: nameof(CancelButton),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry));

        public bool CancelButton
        {
            get => (bool)GetValue(CancelButtonProperty);
            set => SetValue(CancelButtonProperty, value);
        }

        public ICommand TapGestureRecognizer => new Command(OnTapGestureRecognizer);

        private void OnTapGestureRecognizer(object obj)
        {
            cusEntry.Text = string.Empty;
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