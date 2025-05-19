using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Resources.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RyazanSpace.UI.WPF.Resources.Controls
{
    public partial class PostResourcesPresenter : UserControl, INotifyPropertyChanged
    {
        public PostResourcesPresenter()
        {
            InitializeComponent();
        }

        private void Resources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Resources == null) return;
            Images = new ObservableCollection<CloudResourceDTO>(Resources.Where(p => p.Type == CloudResourceType.Image));
            Documents = new ObservableCollection<CloudResourceDTO>(Resources.Where(p => p.Type == CloudResourceType.Document));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Images)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documents)));
        }

        private static void OnSetResources(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PostResourcesPresenter control = d as PostResourcesPresenter;

            control._isReadOnly = false;

            control.Resources_CollectionChanged(null, null);
            if (e.NewValue is ObservableCollection<CloudResourceDTO> observ)
            {
                observ.CollectionChanged += control.Resources_CollectionChanged;
                control._isReadOnly = true;
            }
        }

        public static readonly DependencyProperty ResourcesProperty = 
            DependencyProperty.Register("" +
                "Resources", 
                typeof(ICollection<CloudResourceDTO>), 
                typeof(PostResourcesPresenter),
                new PropertyMetadata(propertyChangedCallback : OnSetResources));

        public event PropertyChangedEventHandler PropertyChanged;

        public new ICollection<CloudResourceDTO> Resources
        {
            get { return (ICollection<CloudResourceDTO>)GetValue(ResourcesProperty); }
            set  { SetValue(ResourcesProperty, value); }
        }

        public ObservableCollection<CloudResourceDTO> Images { get; set; } 
        public ObservableCollection<CloudResourceDTO> Documents { get; set; }

        private bool _isReadOnly;

        private void ScrollToLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //imageScroll.LineLeft();
            imageScroll.ScrollToHorizontalOffset(imageScroll.HorizontalOffset - imageScroll.ViewportWidth);
        }

        private void ScrollToRight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            //imageScroll.LineRight();
            imageScroll.ScrollToHorizontalOffset(imageScroll.HorizontalOffset + imageScroll.ViewportWidth);
        }
    }
}
