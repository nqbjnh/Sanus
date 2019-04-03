using System.Threading.Tasks;
using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class ExtendSplashViewModel : ViewModelBase
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning disable IDE0044 // Add readonly modifier
        INavigationService _navigationService;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        public ExtendSplashViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            await Task.Delay(1000);

            await _navigationService.NavigateAsync("/HomePage");
        }
    }
}
