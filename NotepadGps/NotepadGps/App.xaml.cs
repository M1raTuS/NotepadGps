using NotepadGps.Services.Autentification;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Map;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using NotepadGps.Services.Settings;
using NotepadGps.View;
using NotepadGps.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

namespace NotepadGps
{
    public partial class App : PrismApplication
    {

        private IAutentificationService autentificationService;

        public App()
        {

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAutentificationService>(Container.Resolve<AutentificationService>());
            containerRegistry.RegisterInstance<ISettingsService>(Container.Resolve<SettingsService>());
            containerRegistry.RegisterInstance<IProfileService>(Container.Resolve<ProfileService>());
            containerRegistry.RegisterInstance<IMapPinService>(Container.Resolve<MapPinService>());
            containerRegistry.RegisterInstance<IAutorizationService>(Container.Resolve<AutorizationService>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<MapsPage, MapsPageViewModel>();
            containerRegistry.RegisterForNavigation<ListPage, ListPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditMapPinView, AddEditMapPinViewModel>();
            containerRegistry.RegisterForNavigation<PopUpView, PopUpViewModel>();
        }

        protected override void OnInitialized()
        {
            autentificationService = Container.Resolve<IAutentificationService>();

            if (autentificationService.GetCurrentId != -1)
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}");
            }
            else
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
            }

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
