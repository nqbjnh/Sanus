using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class EnegyHistoryViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        public EnegyHistoryViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
