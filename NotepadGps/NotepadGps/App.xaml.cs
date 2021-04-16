using NotepadGps.Services.Autentification;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.Services.Map;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using NotepadGps.Services.Settings;
using NotepadGps.View;
using NotepadGps.ViewModel;
using Plugin.Media;
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
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<ISettingsService>(Container.Resolve<SettingsService>());
            containerRegistry.RegisterInstance<IAutentificationService>(Container.Resolve<AutentificationService>());
            containerRegistry.RegisterInstance<IProfileService>(Container.Resolve<ProfileService>());
            containerRegistry.RegisterInstance<IAutorizationService>(Container.Resolve<AutorizationService>());
            containerRegistry.RegisterInstance<IMapPinService>(Container.Resolve<MapPinService>());
            containerRegistry.RegisterInstance<IImageService>(Container.Resolve<ImageService>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<MapsPage, MapsPageViewModel>();
            containerRegistry.RegisterForNavigation<ListPage, ListPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditMapPinView, AddEditMapPinViewModel>();
            containerRegistry.RegisterForNavigation<PopUpView, PopUpViewModel>();

            //Packages
            containerRegistry.RegisterInstance(CrossMedia.Current); //TODO: register all packages
        }

        protected override void OnInitialized()
        {
            var autorizationService = Container.Resolve<IAutorizationService>();

            if (autorizationService.IsAutorized)
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
