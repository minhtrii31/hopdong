using System.Windows;

namespace HopDong.Views
{
    /// <summary>
    /// Interaction logic for BaseView.xaml
    /// </summary>
    public partial class BaseView : Window
    {
        public BaseView()
        {
            InitializeComponent();
            this.Loaded += BaseView_Loaded;
        }

        private void BaseView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
