using System.Windows;

namespace HopDong.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is BaseView)
                {
                    window.Close();
                    break;
                }
            }
        }


    }
}
