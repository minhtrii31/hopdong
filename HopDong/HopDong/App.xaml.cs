using HopDong.ViewModels;
using HopDong.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace HopDong
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<BaseView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<BaseView, BaseViewModel>();
            containerRegistry.RegisterForNavigation<ContractType, ContractTypeViewModel>();
            containerRegistry.RegisterForNavigation<TaoHopDong, TaoHopDongViewModel>();
            containerRegistry.RegisterForNavigation<ContractPage, ContractPageViewModel>();
            containerRegistry.RegisterForNavigation<ContractDetail, ContractDetailViewModel>();
            containerRegistry.RegisterForNavigation<ContractDelete, ContractDeleteViewModel>();
            containerRegistry.RegisterForNavigation<ContractUpdate, ContractUpdateViewModel>();
            containerRegistry.RegisterForNavigation<UserPage, UserPageViewModel>();
            containerRegistry.RegisterForNavigation<AddUser, AddUserViewModel>();
            containerRegistry.RegisterForNavigation<UserDelete, UserDeleteViewModel>();
            containerRegistry.RegisterForNavigation<UserDetail, UserDetailViewModel>();
            containerRegistry.RegisterForNavigation<UserUpdate, UserUpdateViewModel>();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var region = ContainerLocator.Container.Resolve<IRegionManager>();
            region.RegisterViewWithRegion<Sidebar>("SidebarRegion");
            region.RegisterViewWithRegion<Home>("ContentRegion");
        }
    }
}
