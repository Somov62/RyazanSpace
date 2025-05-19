using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RyazanSpace.UI.WPF.Resources.Controls
{
    /// <summary>
    /// Логика взаимодействия для CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            InitializeComponent();
            base.Text = string.Empty;
        }

        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register("PlaceHolder", typeof(string), typeof(CustomTextBox));
        public static readonly DependencyProperty PlaceHolderColorProperty = DependencyProperty.Register("PlaceHolderColor", typeof(Brush), typeof(CustomTextBox));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomTextBox));


        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public Brush PlaceHolderColor
        {
            get { return (Brush)GetValue(PlaceHolderColorProperty); }
            set { SetValue(PlaceHolderColorProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}