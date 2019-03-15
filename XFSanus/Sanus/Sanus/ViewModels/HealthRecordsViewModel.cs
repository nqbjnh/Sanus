using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class HealthRecordsViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        public HealthRecordsViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
