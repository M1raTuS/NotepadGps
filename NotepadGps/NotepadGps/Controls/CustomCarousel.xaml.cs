using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NotepadGps.Controls
{
    public partial class CustomCarousel : Xamarin.Forms.ContentView
    {
        public CustomCarousel()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static BindableProperty PositionProperty = BindableProperty.Create(
                                                          propertyName: nameof(Position),
                                                          returnType: typeof(int),
                                                          declaringType: typeof(CustomCarousel),
                                                          defaultValue: 0,
                                                          defaultBindingMode: BindingMode.TwoWay);


        public int Position
        {
            get { return (int)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static BindableProperty SelectedItemProperty = BindableProperty.Create(
                                                              propertyName: nameof(SelectedItem),
                                                              returnType: typeof(object),
                                                              declaringType: typeof(CustomCarousel),
                                                              defaultBindingMode: BindingMode.TwoWay);

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static BindableProperty RootViewModelProperty = BindableProperty.Create(
                                                              propertyName: nameof(RootViewModel),
                                                              returnType: typeof(object),
                                                              declaringType: typeof(CustomCarousel),
                                                              defaultValue: default(object));

        public object RootViewModel
        {
            get { return (object)GetValue(RootViewModelProperty); }
            set { SetValue(RootViewModelProperty, value); }
        }

        public static BindableProperty ItemSourceProperty = BindableProperty.Create(
                                                          propertyName: nameof(ItemSource),
                                                          returnType: typeof(IList),
                                                          declaringType: typeof(StandartCarouselPage),
                                                          defaultBindingMode: BindingMode.TwoWay,
                                                          propertyChanged: UpdateList);


        public IList ItemSource
        {
            get { return (IList)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public static BindableProperty ItemTemplateProperty = BindableProperty.Create(
                                                             propertyName: nameof(ItemTemplate),
                                                             returnType: typeof(DataTemplate),
                                                             declaringType: typeof(CustomCarousel),
                                                             defaultBindingMode: BindingMode.TwoWay);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty ChildProperty = BindableProperty.Create(
                                                                propertyName: nameof(Child),
                                                                returnType: typeof(IList<Page>),
                                                                declaringType: typeof(CustomCarousel),
                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                propertyChanged: UpdateList);

        public IList<Page> Child
        {
            get => (IList<Page>)GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }

        #endregion
        private static void UpdateList(BindableObject bindable, object oldvalue, object newValue)
        {
            
        }
    }
}
