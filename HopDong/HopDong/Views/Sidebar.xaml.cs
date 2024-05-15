using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace HopDong.Views
{
    /// <summary>
    /// Interaction logic for Sidebar
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();
        }
        private void NavigationViewItem_Selected(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as NavigationViewItem)?.Content.ToString();
            var regionManager = Prism.Regions.RegionManager.GetRegionManager(this);

            if (regionManager != null && selectedItem != null)
            {
                switch (selectedItem)
                {
                    case "Trang chính":
                        regionManager.RequestNavigate("ContentRegion", "Home");
                        break;
                    case "Hợp đồng":
                        regionManager.RequestNavigate("ContentRegion", "ContractPage");
                        break;
                    case "Các loại hợp đồng":
                        regionManager.RequestNavigate("ContentRegion", "ContractType");
                        break;
                    case "Thống kê":
                        regionManager.RequestNavigate("ContentRegion", "Report");
                        break;
                    case "Người dùng":
                        regionManager.RequestNavigate("ContentRegion", "UserPage");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
