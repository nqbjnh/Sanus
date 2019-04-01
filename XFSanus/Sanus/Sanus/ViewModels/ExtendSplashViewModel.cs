using System.Threading.Tasks;
using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class ExtendSplashViewModel : ViewModelBase
    {
        INavigationService _navigationService;
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
