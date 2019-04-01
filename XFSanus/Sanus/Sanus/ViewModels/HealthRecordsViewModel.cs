using Prism.Commands;
using Prism.Navigation;
using Sanus.Services.Dialog;

namespace Sanus.ViewModels
{
    public class HealthRecordsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        //
        public DelegateCommand UpdateUserCommand { get; }
        //
        public HealthRecordsViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            //
            UpdateUserCommand = new DelegateCommand(UpdateUser);
        }
        //
        private async void UpdateUser()
        {
            //await _dialogService.ShowConfirmAsync("user click", "Click", "Ok", "Cancel");
            await _navigationService.NavigateAsync("/UserFilesPage");
        }

    }
}
