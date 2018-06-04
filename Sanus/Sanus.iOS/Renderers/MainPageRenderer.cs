using Sanus.iOS.Renderers;
using Sanus.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MainPage), typeof(MainPageRenderer))]
namespace Sanus.iOS.Renderers
{
    public class MainPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            TabBar.TintColor = UIColor.FromRGBA(35, 184, 249, 255);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (TabBar.Items != null)
            {
                var items = TabBar.Items;
                foreach (UITabBarItem item in items)
                {
                    item.Image = item.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    item.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
                    item.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.FromRGBA(35, 184, 249, 255) }, UIControlState.Selected);
                }
            }
        }
    }
}