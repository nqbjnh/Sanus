using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms.Xaml;
using Sanus.ViewModels;

namespace Sanus.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendNavigationBar
    {
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(ExtendNavigationBar),
                default(string),
                BindingMode.OneWay,
                null,
                TitleChanged);

        public static readonly BindableProperty HasBackButtonProperty =
            BindableProperty.Create(
                nameof(HasBackButton),
                typeof(bool),
                typeof(ExtendNavigationBar),
                true,
                BindingMode.OneWay,
                null,
                HasBackButtonChanged);

        public static readonly BindableProperty ActionIconProperty =
           BindableProperty.Create(
               nameof(ActionIcon),
               typeof(string),
               typeof(ExtendNavigationBar),
               default(string),
               BindingMode.OneWay,
               null,
               ActionIconChanged);

        public static readonly BindableProperty ActionCommandProperty =
         BindableProperty.Create(nameof(ActionCommand), typeof(ICommand), typeof(ExtendNavigationBar));

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExtendNavigationBar));

        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public bool HasBackButton { get => (bool)GetValue(HasBackButtonProperty); set => SetValue(HasBackButtonProperty, value); }
        public string ActionIcon { get => (string)GetValue(ActionIconProperty); set => SetValue(ActionIconProperty, value); }
        public ICommand ActionCommand { get => (ICommand)GetValue(ActionCommandProperty); set => SetValue(ActionCommandProperty, value); }
        public object CommandParameter { get => (object)GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        public IList<View> LeftView
        {
            get => LeftLayout.Children;
            set
            {
                if (!LeftLayout.Children.Equals(value))
                {
                    LeftLayout.Children.Clear();
                    if (value != null)
                    {
                        foreach (var view in value)
                        {
                            LeftLayout.Children.Add(view);
                        }
                    }
                }
            }
        }

        protected IList<View> CenterView => CenterLayout.Children;
        public IList<View> RightView
        {
            get => RightLayout.Children;
            set
            {
                if (!RightLayout.Children.Equals(value))
                {
                    RightLayout.Children.Clear();
                    if (value != null)
                    {
                        foreach (var view in value)
                        {
                            RightLayout.Children.Add(view);
                        }
                    }
                }
            }
        }
        private Label TitleView { get; set; }
        private ContentView BackButton { get; set; }
        private ContentView ActionButton { get; set; }

        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bar = (ExtendNavigationBar)bindable;
            if (!string.IsNullOrEmpty(newValue?.ToString()))
            {
                if (bar.TitleView == null)
                {
                    bar.CreateTitleView();
                }
                bar.TitleView.Text = newValue.ToString();

            }
        }

        private static void ActionIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bar = (ExtendNavigationBar)bindable;
            if (!string.IsNullOrEmpty(newValue?.ToString()))
            {
                if (bar.ActionButton == null)
                {
                    bar.CreateActionButton();
                }

            }
        }

        private static void HasBackButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bar = (ExtendNavigationBar)bindable;
            var hasBackButton = (bool)newValue;
            bar.BackButton.IsVisible = hasBackButton;
        }

        public ExtendNavigationBar()
        {
            InitializeComponent();
            InitAsDefault();
        }

        private void InitAsDefault()
        {
            // title
            if (!string.IsNullOrEmpty(Title))
            {
                CreateTitleView();
            }
            //Action Button
            if (!string.IsNullOrEmpty(ActionIcon))
            {
                CreateActionButton();
            }
            // back button
            BackButton = new ContentView
            {
                InputTransparent = false,
                Padding = new Thickness(10, 0),
                VerticalOptions = LayoutOptions.Center,
                Content = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 24,
                    HeightRequest = 24,
                    InputTransparent = true,
                    Aspect = Aspect.AspectFit,
                    Source = "ic_arrow_back.png"
                }
            };

            BackButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { (BindingContext as ViewModelBase)?.BackCommand.Execute(); })
            });

            LeftView.Add(BackButton);
        }

        private void CreateActionButton()
        {
            ActionButton = new ContentView
            {
                InputTransparent = false,
                Padding = new Thickness(0, 10, 20, 10),
                Content = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 20,
                    HeightRequest = 20,

                    InputTransparent = true,
                    Aspect = Aspect.AspectFit,
                    Source = ActionIcon,
                    Transformations = new List<ITransformation>() { new TintTransformation("#ffffff") { EnableSolidColor = true } }
                }
            };

            ActionButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { ActionCommand?.Execute(CommandParameter); })
            });
            RightView.Add(ActionButton);
        }

        private void CreateTitleView()
        {
            TitleView = new Label
            {
                Style = Application.Current.Resources["NavigationTitleLabel"] as Style,
                LineBreakMode = LineBreakMode.TailTruncation,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 0),
            };
            CenterView.Add(TitleView);
        }
    }
}