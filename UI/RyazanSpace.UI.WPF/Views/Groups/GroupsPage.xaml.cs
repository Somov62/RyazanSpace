using RyazanSpace.UI.WPF.ViewModels.Groups;
using System.Windows.Controls;

namespace RyazanSpace.UI.WPF.Views.Groups
{
    public partial class GroupsPage : Page
    {
        public GroupsPage()
        {
            InitializeComponent();
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset == 0) return;
            if (e.VerticalOffset + e.ViewportHeight > e.ExtentHeight - 200)
            {
                (this.DataContext as GroupsViewModel).LoadNextPageCommand.Execute(null);
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (this.DataContext as GroupsViewModel).OpenGroupCommand.Execute((sender as ListBoxItem).DataContext);
        }
    }
}
