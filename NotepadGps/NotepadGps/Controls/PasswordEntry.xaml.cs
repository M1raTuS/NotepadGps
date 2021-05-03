﻿using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.Controls
{
    public partial class PasswordEntry : Grid
    {
        public PasswordEntry()
        {
            InitializeComponent();
            img.IsVisible = false;
        }

        #region -- Public properties --

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                               propertyName: nameof(Text),
                                                               returnType: typeof(string),
                                                               declaringType: typeof(PasswordEntry),
                                                               defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                                      propertyName: nameof(Placeholder),
                                                                      returnType: typeof(string),
                                                                      declaringType: typeof(PasswordEntry));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(PlaceholderColor),
                                                                           returnType: typeof(Color),
                                                                           declaringType: typeof(PasswordEntry));

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty LblTextProperty = BindableProperty.Create(
                                                                  propertyName: nameof(LblText),
                                                                  returnType: typeof(string),
                                                                  declaringType: typeof(PasswordEntry));

        public string LblText
        {
            get => (string)GetValue(LblTextProperty);
            set => SetValue(LblTextProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
                                                                    propertyName: nameof(ErrorText),
                                                                    returnType: typeof(string),
                                                                    declaringType: typeof(PasswordEntry));

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public static readonly BindableProperty IsErrorVisibleProperty = BindableProperty.Create(
                                                                         propertyName: nameof(IsErrorVisible),
                                                                         returnType: typeof(bool),
                                                                         declaringType: typeof(PasswordEntry));

        public bool IsErrorVisible
        {
            get => (bool)GetValue(IsErrorVisibleProperty);
            set => SetValue(IsErrorVisibleProperty, value);
        }

        public static readonly BindableProperty FrameBorderColorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(FrameBorderColor),
                                                                           returnType: typeof(Color),
                                                                           declaringType: typeof(PasswordEntry));

        public Color FrameBorderColor
        {
            get => (Color)GetValue(FrameBorderColorProperty);
            set => SetValue(FrameBorderColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
                                                                   propertyName: nameof(TextColor),
                                                                   returnType: typeof(Color),
                                                                   declaringType: typeof(PasswordEntry));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public ICommand TapGestureRecognizer => new Command(OnTapGestureRecognizer);

        private void OnTapGestureRecognizer(object obj)
        {
            if (eye.IsPassword)
            {
                img.Source = "ic_eye";
                eye.IsPassword = false;
            }
            else
            {
                img.Source = "ic_eye_off";
                eye.IsPassword = true;
            }
        }

        #endregion

        #region -- Overrides -- 

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Text))
            {
                if (string.IsNullOrWhiteSpace(Text))
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