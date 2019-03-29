using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sanus.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public DelegateCommand BackCommand { get; }
        public bool Initialized { get; set; }

        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
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
        protected virtual void GoBack()
        {
            NavigationService.GoBackAsync();
        }

        public virtual void Destroy()
        {

        }
    }
}
