using RyazanSpace.UI.WPF.ViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
