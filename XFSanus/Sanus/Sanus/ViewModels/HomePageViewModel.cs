using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace Sanus.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
