using Prism.Navigation;
using Sanus.Services.Dialog;

namespace Sanus.ViewModels
{
    public class UserFilesViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private INavigationService _navigationService;
        public UserFilesViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
        }
    }
}
