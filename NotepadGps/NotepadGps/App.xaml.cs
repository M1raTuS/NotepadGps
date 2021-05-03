using NotepadGps.Services.Autentification;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.Services.Map;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using NotepadGps.Services.Settings;
using NotepadGps.Services.Theme;
using NotepadGps.View;
using NotepadGps.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

namespace NotepadGps
{
    public partial class App : PrismApplication
    {
        public App() { }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsService>(Container.Resolve<SettingsService>());
            containerRegistry.RegisterInstance<IAutentificationService>(Container.Resolve<AutentificationService>());
            containerRegistry.RegisterInstance<IProfileService>(Container.Resolve<ProfileService>());
            containerRegistry.RegisterInstance<IAutorizationService>(Container.Resolve<AutorizationService>());
            containerRegistry.RegisterInstance<IMapPinService>(Container.Resolve<MapPinService>());
            containerRegistry.RegisterInstance<IImageService>(Container.Resolve<ImageService>());
            containerRegistry.RegisterInstance<IEventService>(Container.Resolve<EventService>());
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPageView, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPwCheckView, SignUpPwCheckViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<EventsPinsView, EventsPinsViewModel>();
            containerRegistry.RegisterForNavigation<MapsPageView, MapsPageViewModel>();
            containerRegistry.RegisterForNavigation<ListPageView, ListPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditMapPinView, AddEditMapPinViewModel>();
            containerRegistry.RegisterForNavigation<PopUpView, PopUpViewModel>();
            containerRegistry.RegisterForNavigation<EventsView, EventsViewModel>();
            containerRegistry.RegisterForNavigation<NotifyPageView, NotifyViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        }

        protected override void OnInitialized()
        {
            base.Initialize();

            var autorizationService = Container.Resolve<IAutorizationService>();

            if (autorizationService.IsAutorized)
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}");
            }
            else
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPageView)}");
            }
        }

        protected override void OnStart()
        {
            var themeService = Container.Resolve<IThemeService>();
            themeService.LoadTheme();
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
