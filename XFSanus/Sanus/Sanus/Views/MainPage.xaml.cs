using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanus.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.Detail = new NavigationPage(new HomePage());
        }
    }
}