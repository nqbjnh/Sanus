using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sanus.Controls
{
    public class Custom
    {
        private class PageControlTemplate : ExtendNavigation
        {
            public PageControlTemplate()
            {
                LayoutChanged += OnLayoutChanged;
            }
        }

        private static void OnLayoutChanged(object sender, EventArgs e)
        {
            if (((ExtendNavigation)sender).NavigationBar == null &&
                ((ExtendNavigation)sender).Parent is ContentPage parent)
            {
                var navigationBar = GetNavigationBar(parent);

                //parent.Appearing += (s, agrs) => {

                //    var insets = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.GetSafeAreaInsets(parent);
                //    if (insets.Top > 0 && navigationBar != null)
                //    {
                //        navigationBar.Padding = new Thickness(0, insets.Top, 0, 0);
                //        navigationBar.ForceLayout();
                //        ((ExtendNavigation)sender).ForceLayout();
                //    }
                //};



                if (navigationBar != null && navigationBar.BindingContext != parent.BindingContext)
                {
                    navigationBar.Padding = new Thickness(0, App.Insets.Top, 0, 0);
                    navigationBar.BindingContext = parent.BindingContext;
                }

                if (navigationBar != null)
                {
                    ((ExtendNavigation)sender).NavigationBar = navigationBar;
                    ((ExtendNavigation)sender).ForceLayout();
                }
            }
        }

        public static readonly BindableProperty NavigationBarProperty =
            BindableProperty.CreateAttached(
                "NavigationBar",
                typeof(ExtendNavigationBar),
                typeof(Custom),
                default(ExtendNavigationBar),
                BindingMode.OneWay,
                null,
                OnNavigationBarPropertyChanged);


        public static readonly BindableProperty EntryCommandProperty =
            BindableProperty.CreateAttached(
                "EntryCommand",
                typeof(ICommand),
                typeof(Custom),
                null,
                BindingMode.OneWay,
                null,
                OnEntryCommandPropertyChanged);

        private static void OnEntryCommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Entry entry && newvalue is ICommand command)
            {
                entry.Completed += (s, e) => { command.Execute(entry.Text); };
            }
        }

        public static ExtendNavigationBar GetNavigationBar(BindableObject bindableObject)
        {
            return (ExtendNavigationBar)bindableObject.GetValue(NavigationBarProperty);
        }

        public static void SetNavigationBar(BindableObject bindableObject, object value)
        {
            bindableObject.SetValue(NavigationBarProperty, value);
        }

        private static void OnNavigationBarPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Page page)
            {
                try
                {
                    NavigationPage.SetHasNavigationBar(page, false);
                    NavigationPage.SetHasBackButton(page, false);
                }
                catch (Exception)
                {
                    //throw;
                }


                if (bindable is TemplatedPage templatedPage)
                {
                    templatedPage.ControlTemplate = new ControlTemplate(typeof(PageControlTemplate));
                }

                if (Device.RuntimePlatform == Device.iOS)
                {
                    page.PropertyChanged -= PagePropertyChanged;
                    page.PropertyChanged += PagePropertyChanged;
                    page.Appearing += PageAppearing;
                    Safe(page);
                }
            }
        }

        private static void PageAppearing(object sender, EventArgs e)
        {
            if (sender is Page page)
            {
                page.Appearing -= PageAppearing;
                //Safe(page);
            }
        }

        private static void PagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Page page && e.PropertyName == "SafeAreaInsets")
            {
                //Safe(page);

                page.PropertyChanged -= PagePropertyChanged;
            }
        }

        public static void Safe(Page page)
        {
            var insets = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.GetSafeAreaInsets(page);

            var bar = GetNavigationBar(page);

            if (bar != null && insets.Top > 0)
            {
                bar.Padding = new Thickness(0, insets.Top, 0, 0);
                //  // bar.HeightRequest = 50 + insets.Top;
            }
        }
    }
}
