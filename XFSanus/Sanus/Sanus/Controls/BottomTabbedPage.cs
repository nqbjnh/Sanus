using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Sanus.Controls
{
    public class BottomTabbedPage : Xamarin.Forms.TabbedPage
    {
        public BottomTabbedPage()
        {
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}
