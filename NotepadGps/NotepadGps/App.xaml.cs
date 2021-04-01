using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using NotepadGps.View;
using NotepadGps.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

namespace NotepadGps
{
    public partial class App : PrismApplication
    {
        public App()
        {

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAutorizationService>(Container.Resolve<AutorizationService>());
            containerRegistry.RegisterInstance<IProfileService>(Container.Resolve<ProfileService>());


            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<MapsPage1,MapsPage1ViewModel>();
            containerRegistry.RegisterForNavigation<MapsPage2>();

        }

        protected override void OnInitialized()
        {
            InitializeComponent();

           //NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}");
            NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
