using RyazanSpace.UI.WPF.ViewModels.Groups;
using System.Windows.Controls;

namespace RyazanSpace.UI.WPF.Views.Groups
{
    /// <summary>
    /// Логика взаимодействия для GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        public GroupPage()
        {
            InitializeComponent();
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset == 0) return;
            if (e.VerticalOffset + e.ViewportHeight > e.ExtentHeight - 200)
            {
                (this.DataContext as GroupViewModel).LoadNextPageCommand.Execute(null);
            }
        }
    }
}
