using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sanus.Services.Dialog;
using Sanus.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sanus.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService _navigationService { get; private set; }

        private string _title;
        public DelegateCommand BackCommand { get; }
        public bool Initialized { get; set; }

        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }

        public ViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
            //
            BackCommand = new DelegateCommand(GoBack);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }
        public virtual Task OnNavigationAsync(NavigationParameters parameter)
        {
            return Task.CompletedTask;
        }
        public async virtual void GoBack()
        {
            await _navigationService.GoBackAsync(null, true, true); ;
        }

        public virtual void Destroy()
        {

        }
    }
}
