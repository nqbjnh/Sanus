using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanus.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplitViewThreeTab : Grid
    {
        private View _firstView;
        private View _secondView;
        private View _threeView;
        //
        public static readonly BindableProperty CurrentViewProperty = BindableProperty.Create(nameof(CurrentView), typeof(int), typeof(SplitViewThreeTab), 0);

        public static readonly BindableProperty TitleProperty = BindableProperty.CreateAttached("Title", typeof(string), typeof(SplitViewThreeTab), null, BindingMode.OneWay, null, propertyChanged: TitleChanged);

        public static readonly BindableProperty SelectCommandProperty = BindableProperty.Create(nameof(SelectCommand), typeof(ICommand), typeof(SplitViewThreeTab), default(ICommand));

        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is View view && view.Parent is SplitViewThreeTab splitView && splitView != null)
            {
                if (splitView.FirstView == view)
                {
                    splitView.FirstTitleLabel2.Text = newValue?.ToString();
                }
                else
                {
                    splitView.SecondTitleLabel2.Text = GetTitle(splitView.SecondView);
                    splitView.ThreeTitleLabel2.Text = GetTitle(splitView.ThreeView);
                }

                if (splitView.SecondView == view)
                {
                    splitView.SecondTitleLabel2.Text = newValue?.ToString();
                }
                else
                {
                    splitView.FirstTitleLabel2.Text = GetTitle(splitView.FirstView);
                    splitView.ThreeTitleLabel2.Text = GetTitle(splitView.ThreeView);
                }

                if (splitView.ThreeView == view)
                {
                    splitView.ThreeTitleLabel2.Text = newValue?.ToString();
                }
                else
                {
                    splitView.SecondTitleLabel2.Text = GetTitle(splitView.SecondView);
                    splitView.FirstTitleLabel2.Text = GetTitle(splitView.FirstView);
                }
            }
        }

        public static string GetTitle(BindableObject view)
        {
            return (string)view.GetValue(TitleProperty);
        }

        public static void SetTitle(BindableObject view, string value)
        {
            view.SetValue(TitleProperty, value);
        }

        public View FirstView
        {
            get => _firstView;
            set
            {
                if (_firstView != value)
                {
                    if (_firstView != null)
                        this.Children.Remove(_firstView);

                    _firstView = value;
                    if (_firstView != null)
                    {
                        SetRow(_firstView, 2);
                        SetColumnSpan(_firstView, 3);

                        _firstView.IsVisible = CurrentView == 0;

                        FirstTitleLabel2.Text = GetTitle(_firstView);

                        Children.Add(_firstView);
                    }
                }
            }
        }

        public View SecondView
        {
            get => _secondView;
            set
            {
                if (_secondView != value)
                {
                    if (_secondView != null)
                        this.Children.Remove(_secondView);

                    _secondView = value;
                    if (_secondView != null)
                    {
                        SetRow(_secondView, 2);
                        SetColumnSpan(_secondView, 3);

                        _secondView.IsVisible = CurrentView == 1;

                        SecondTitleLabel2.Text = GetTitle(_secondView);

                        this.Children.Add(_secondView);
                    }
                }
            }
        }

        public View ThreeView
        {
            get => _threeView;
            set
            {
                if (_threeView != value)
                {
                    if (_threeView != null)
                        this.Children.Remove(_threeView);

                    _threeView = value;
                    if (_threeView != null)
                    {
                        SetRow(_threeView, 2);
                        SetColumnSpan(_threeView, 3);

                        _threeView.IsVisible = CurrentView == 2;

                        ThreeTitleLabel2.Text = GetTitle(_threeView);

                        this.Children.Add(_threeView);
                    }
                }
            }
        }

        public int CurrentView
        {
            get => (int)GetValue(CurrentViewProperty);
            set => SetValue(CurrentViewProperty, value);
        }

        public ICommand SelectCommand
        {
            get => (ICommand)GetValue(SelectCommandProperty);
            set => SetValue(SelectCommandProperty, value);
        }

        public SplitViewThreeTab()
        {
            InitializeComponent();
        }

        private void UpdateView()
        {
            if (FirstView != null)
                FirstView.IsVisible = CurrentView == 0;
            if (SecondView != null)
                SecondView.IsVisible = CurrentView == 1;
            if (ThreeView != null)
                ThreeView.IsVisible = CurrentView == 2;

            SelectCommand?.Execute(CurrentView);
        }

        private void FirstTabTapped(object sender, EventArgs e)
        {
            CurrentView = 0;
            UpdateView();
        }

        private void SecondTabTapped(object sender, EventArgs e)
        {
            CurrentView = 1;
            UpdateView();
        }

        private void ThreeTabTapped(object sender, EventArgs e)
        {
            CurrentView = 2;
            UpdateView();
        }
    }
}