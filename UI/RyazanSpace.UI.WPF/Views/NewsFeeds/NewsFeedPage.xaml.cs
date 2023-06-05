using RyazanSpace.UI.WPF.ViewModels.Groups;
using RyazanSpace.UI.WPF.ViewModels.NewsFeeds;
using System.Windows.Controls;

namespace RyazanSpace.UI.WPF.Views.NewsFeeds
{
    public partial class NewsFeedPage : Page
    {
        public NewsFeedPage()
        {
            InitializeComponent();
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset == 0) return;
            if (e.VerticalOffset + e.ViewportHeight > e.ExtentHeight - 200)
            {
                (this.DataContext as NewsFeedViewModel).LoadNextPageCommand.Execute(null);
            }
        }
    }
}
