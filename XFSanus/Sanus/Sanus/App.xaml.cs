using Prism;
using Prism.Ioc;
using Sanus.Services.Charts;
using Sanus.Services.Health;
using Sanus.ViewModels;
using Sanus.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sanus
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/ExtendSplashPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<ExtendSplashPage, ExtendSplashViewModel>();
            containerRegistry.RegisterForNavigation<HealthRecordsPage, HealthRecordsViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<DemoPage, DemoPageViewModel>();
            //
            //var googleFit = DependencyService.Get<IHealthServices>();
            //containerRegistry.RegisterInstance<IHealthServices>(googleFit);
            //
            containerRegistry.Register<IChartService, ShowChartService>();
        }
    }
}
