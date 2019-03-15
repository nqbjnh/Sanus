using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StepCounter
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var myStack = new StackLayout();
            Content = myStack;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchHealthData();

            // iterate over a collection adding tasks to the dispatch group

        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            //StackLayout retainer = new StackLayout();

            this.myStack.Children.Clear();
            FetchHealthData();
        }

        void OnButtonStartClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHealthData>().StartSubscription((isStart) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label label = new Label
                    {
                        Text = "Subscription: " + isStart,
                    };
                    this.myStack.Children.Add(label);
                });
            });
            Content = myStack;
        }

        void OnButtonStopClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHealthData>().CancelSubscription((isStop) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label label = new Label
                    {
                        Text = "Cancel Subscription: " + isStop,
                    };
                    this.myStack.Children.Add(label);
                });
            });

            Content = myStack;
        }

        void FetchHealthData()
        {

            List<Task> tasks = new List<Task>();
            DependencyService.Get<IHealthData>().GetHealthPermissionAsync((result) =>
            {
                var a = result;
                if (result)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(1), GetData);
                    //GetData();
                }
            });

        }

        public bool GetData()
        {
            this.myStack.Children.Clear();

            DependencyService.Get<IHealthData>().FetchSteps((totalSteps) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label label = new Label
                    {
                        Text = "Total steps today: " + Math.Floor(totalSteps).ToString() + " time: " + DateTime.Now.ToString("HH:mm:ss"),
                    };
                    ScrollView scroll = new ScrollView();
                    StackLayout newStack = new StackLayout();

                    this.myStack.Children.Add(label);
                    //this.   .Add(label);
                });
            });

            DependencyService.Get<IHealthData>().FetchMetersWalked((metersWalked) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label label = new Label
                    {
                        Text = "Total meters walked today: " + Math.Floor(metersWalked).ToString(),
                    };
                    ScrollView scroll = new ScrollView();
                    StackLayout newStack = new StackLayout();

                    this.myStack.Children.Add(label);
                });

            });

            DependencyService.Get<IHealthData>().FetchActiveMinutes((activeMinutes) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label label = new Label
                    {
                        Text = "Total excersice minutes today: " + Math.Floor(activeMinutes).ToString(),
                    };
                    ScrollView scroll = new ScrollView();
                    StackLayout newStack = new StackLayout();

                    this.myStack.Children.Add(label);
                });

            });

            DependencyService.Get<IHealthData>().FetchActiveEnergyBurned((caloriesBurned) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label label = new Label
                    {
                        Text = "Total active calories burned today: " + Math.Floor(caloriesBurned).ToString(),
                    };
                    ScrollView scroll = new ScrollView();
                    StackLayout newStack = new StackLayout();

                    this.myStack.Children.Add(label);
                });

            });

            // wait for them all to finish
            Device.BeginInvokeOnMainThread(() =>
            {

                Button btn = new Button
                {
                    Text = "Refresh"
                };
                Button btnStart = new Button
                {
                    Text = "Start"
                };
                Button btnStop = new Button
                {
                    Text = "Stop"
                };
                btn.Clicked += OnButtonClicked;
                btnStart.Clicked += OnButtonStartClicked;
                btnStop.Clicked += OnButtonStopClicked;

                this.myStack.Children.Add(btn);
                this.myStack.Children.Add(btnStart);
                this.myStack.Children.Add(btnStop);

                //this.Label1.Text = "Total steps today: " + Math.Floor(steps).ToString() + " Meters Walked " + Math.Floor(meters).ToString() + " Active minutes " + Math.Floor(minutes).ToString();
            });
            Content = myStack;
            return true;
        }
    }
}
