using Xamarin.Forms;

namespace Sanus.Controls
{
    public class ExtendNavigation : Grid
    {
        private ExtendNavigationBar _navigationBar;

        public ExtendNavigationBar NavigationBar
        {
            get => _navigationBar;
            set
            {
                if (_navigationBar != value && _navigationBar != null)
                    Children.Remove(_navigationBar);

                _navigationBar = value;
                if (_navigationBar != null)
                    Children.Add(_navigationBar);
            }
        }

        protected ExtendNavigation()
        {
            ColumnSpacing = 0;
            RowSpacing = 0;
            RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            RowDefinitions.Add(new RowDefinition());
            var cp = new ContentPresenter();
            SetRow(cp, 1);
            Children.Add(cp);
        }
    }
}
