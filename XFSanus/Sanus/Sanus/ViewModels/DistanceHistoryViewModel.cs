using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class DistanceHistoryViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        public DistanceHistoryViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
