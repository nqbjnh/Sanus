using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        public ProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
