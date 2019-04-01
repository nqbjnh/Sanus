using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace Sanus.Services.Dialog
{
    public class DialogService : IDialogService
    {
        bool loadingShown = false;
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            if (loadingShown)
                HideLoading();
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public void ShowToast(string message, int duration = 5000)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(duration);
            toastConfig.Position = Device.RuntimePlatform == Device.UWP ? ToastPosition.Top : ToastPosition.Bottom;
            UserDialogs.Instance.Toast(toastConfig);
        }

        public Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel)
        {
            if (loadingShown)
                HideLoading();
            return UserDialogs.Instance.ConfirmAsync(message, title, okLabel, cancelLabel);
        }

        public Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options)
        {
            return SelectActionAsync(message, title, "Cancel", options);
        }

        public async Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> options)
        {
            try
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                var optionsArray = options as string[] ?? options.ToArray();

                if (!optionsArray.Any())
                {
                    throw new ArgumentException("No options provided", nameof(options));
                }

                var result =
                    await UserDialogs.Instance.ActionSheetAsync(message, cancelLabel, null, buttons: optionsArray.ToArray());

                return optionsArray.Contains(result)
                    ? result
                    : cancelLabel;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<int> DisplayActionSheetAsync(string title, string cancelButton, string destroyButton, params string[] otherButtons)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync(title, cancelButton, destroyButton, null, otherButtons);

            for (int i = 0; i < otherButtons.Length; i++)
            {
                if (otherButtons[i] == result)
                    return i;
            }
            return -1;
        }

        public void ShowLoading(string message = null)
        {
            if (loadingShown)
                return;
            loadingShown = true;
            UserDialogs.Instance.ShowLoading(message);
        }

        public void HideLoading()
        {
            if (loadingShown)
            {
                loadingShown = false;
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
