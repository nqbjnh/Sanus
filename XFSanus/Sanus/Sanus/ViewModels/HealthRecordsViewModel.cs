using Prism.Commands;
using Prism.Navigation;
using Sanus.Services.Dialog;

namespace Sanus.ViewModels
{
    public class HealthRecordsViewModel : ViewModelBase
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning disable IDE0044 // Add readonly modifier
        INavigationService _navigationService;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        //
        public DelegateCommand UpdateUserCommand { get; }
        //
        public HealthRecordsViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            //
            UpdateUserCommand = new DelegateCommand(UpdateUser);
        }
        //
        private async void UpdateUser()
        {
            await _navigationService.NavigateAsync("/UserFilesPage");
        }

    }
}
