using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
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
        public static Thickness Insets { get; set; } = new Thickness(0, 0, 0, 0);

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            //
            InitializeComponent();
            //
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                Insets = new Thickness(0, 20, 0, 0);
            }
            //
            AppCenter.Start("android=089f427d-f2fb-436d-a21f-c1a16462acc0;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
            //
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
            containerRegistry.RegisterForNavigation<UserFilesPage, UserFilesViewModel>();
          
            containerRegistry.RegisterForNavigation<EnegyHistoryPage, EnegyHistoryViewModel>();
            containerRegistry.RegisterForNavigation<DistanceHistoryPage, DistanceHistoryViewModel>();
            containerRegistry.RegisterForNavigation<StepsHistoryPage, StepsHistoryViewModel>();
            //
            //var googleFit = DependencyService.Get<IHealthServices>();
            //containerRegistry.RegisterInstance<IHealthServices>(googleFit);
            //
            containerRegistry.Register<IChartService, ShowChartService>();
            containerRegistry.Register<IDialogService, DialogService>();
        }
    }
}
